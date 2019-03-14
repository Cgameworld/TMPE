﻿using ColossalFramework;
using TrafficManager.RedirectionFramework.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using TrafficManager.Custom.PathFinding;
using TrafficManager.Geometry;
using TrafficManager.Manager;
using TrafficManager.Traffic;
using TrafficManager.Traffic.Data;
using TrafficManager.Traffic.Enums;
using UnityEngine;
using static TrafficManager.Custom.PathFinding.CustomPathManager;

namespace TrafficManager.Custom.AI {
	[TargetType(typeof(TaxiAI))]
	public class CustomTaxiAI : CarAI {
		[RedirectMethod]
		public bool CustomStartPathFind(ushort vehicleID, ref Vehicle vehicleData, Vector3 startPos, Vector3 endPos, bool startBothWays, bool endBothWays, bool undergroundTarget) {
			CitizenManager instance = Singleton<CitizenManager>.instance;
			ushort passengerInstanceId = Constants.ManagerFactory.ExtVehicleManager.GetDriverInstanceId(vehicleID, ref vehicleData);
			if (passengerInstanceId == 0 || (instance.m_instances.m_buffer[(int)passengerInstanceId].m_flags & CitizenInstance.Flags.Character) != CitizenInstance.Flags.None) {
				return base.StartPathFind(vehicleID, ref vehicleData, startPos, endPos, startBothWays, endBothWays, undergroundTarget);
			}
			VehicleInfo info = this.m_info;
			CitizenInfo info2 = instance.m_instances.m_buffer[(int)passengerInstanceId].Info;
			NetInfo.LaneType laneTypes = NetInfo.LaneType.Vehicle | NetInfo.LaneType.Pedestrian | NetInfo.LaneType.TransportVehicle;
			VehicleInfo.VehicleType vehicleTypes = this.m_info.m_vehicleType;
			bool allowUnderground = (vehicleData.m_flags & Vehicle.Flags.Underground) != 0;
			PathUnit.Position startPosA;
			PathUnit.Position startPosB;
			float startSqrDistA;
			float startSqrDistB;
			PathUnit.Position endPosA;
			if (CustomPathManager.FindPathPosition(startPos, ItemClass.Service.Road, NetInfo.LaneType.Vehicle | NetInfo.LaneType.TransportVehicle, info.m_vehicleType, allowUnderground, false, 32f, out startPosA, out startPosB, out startSqrDistA, out startSqrDistB) &&
				Constants.ManagerFactory.ExtCitizenInstanceManager.FindEndPathPosition(passengerInstanceId, ref instance.m_instances.m_buffer[(int)passengerInstanceId], endPos, laneTypes, vehicleTypes, undergroundTarget, out endPosA)) {
				if ((instance.m_instances.m_buffer[(int)passengerInstanceId].m_flags & CitizenInstance.Flags.CannotUseTransport) == CitizenInstance.Flags.None) {
					laneTypes |= NetInfo.LaneType.PublicTransport;

					uint citizenId = instance.m_instances.m_buffer[passengerInstanceId].m_citizen;
					if (citizenId != 0u && (instance.m_citizens.m_buffer[citizenId].m_flags & Citizen.Flags.Evacuating) != Citizen.Flags.None) {
						laneTypes |= NetInfo.LaneType.EvacuationTransport;
					}
				}
				if (!startBothWays || startSqrDistA < 10f) {
					startPosB = default(PathUnit.Position);
				}
				PathUnit.Position endPosB = default(PathUnit.Position);
				SimulationManager simMan = Singleton<SimulationManager>.instance;
				uint path;
				// NON-STOCK CODE START
				PathCreationArgs args;
				args.extPathType = ExtPathType.None;
				args.extVehicleType = ExtVehicleType.Taxi;
				args.vehicleId = vehicleID;
				args.spawned = (vehicleData.m_flags & Vehicle.Flags.Spawned) != 0;
				args.buildIndex = simMan.m_currentBuildIndex;
				args.startPosA = startPosA;
				args.startPosB = startPosB;
				args.endPosA = endPosA;
				args.endPosB = endPosB;
				args.vehiclePosition = default(PathUnit.Position);
				args.laneTypes = laneTypes;
				args.vehicleTypes = vehicleTypes;
				args.maxLength = 20000f;
				args.isHeavyVehicle = this.IsHeavyVehicle();
				args.hasCombustionEngine = this.CombustionEngine();
				args.ignoreBlocked = this.IgnoreBlocked(vehicleID, ref vehicleData);
				args.ignoreFlooded = false;
				args.ignoreCosts = false;
				args.randomParking = false;
				args.stablePath = false;
				args.skipQueue = (vehicleData.m_flags & Vehicle.Flags.Spawned) != 0;

				if (CustomPathManager._instance.CustomCreatePath(out path, ref simMan.m_randomizer, args)) {
					// NON-STOCK CODE END
					if (vehicleData.m_path != 0u) {
						Singleton<PathManager>.instance.ReleasePath(vehicleData.m_path);
					}
					vehicleData.m_path = path;
					vehicleData.m_flags |= Vehicle.Flags.WaitingPath;
					return true;
				}
			}
			return false;
		}

	}
}
