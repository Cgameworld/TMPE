﻿using ColossalFramework;
using ColossalFramework.Math;
using CSUtil.Commons.Benchmark;
using TrafficManager.RedirectionFramework.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrafficManager.Manager;
using TrafficManager.Manager.Impl;
using TrafficManager.State;
using TrafficManager.Traffic;
using TrafficManager.UI;
using UnityEngine;

namespace TrafficManager.Custom.AI {
	[TargetType(typeof(BuildingAI))]
	public class CustomBuildingAI : BuildingAI {
		[RedirectMethod]
		public Color CustomGetColor(ushort buildingID, ref Building data, InfoManager.InfoMode infoMode) {
			if (infoMode != InfoManager.InfoMode.None) {
				// NON-STOCK CODE START
				/*
				 * When the Parking AI is enabled
				 * and the traffic info view is active, colorizes buildings depending on the number of succeeded/failed parking maneuvers, or
				 * if the public transport info view is active, colorizes buildings depending on the current unfulfilled incoming/outgoing demand for public transport.
				 */
				if (Options.parkingAI) {
					Color? color;
					if (AdvancedParkingManager.Instance.GetBuildingInfoViewColor(buildingID, ref data, ref ExtBuildingManager.Instance.ExtBuildings[buildingID], infoMode, out color)) {
						return (Color)color;
					}
				}
				// NON-STOCK CODE END

				return Singleton<InfoManager>.instance.m_properties.m_neutralColor;
			}

			if (!this.m_info.m_useColorVariations) {
				return this.m_info.m_color0;
			}

			Randomizer randomizer = new Randomizer((int)buildingID);
			switch (randomizer.Int32(4u)) {
				case 0:
					return this.m_info.m_color0;
				case 1:
					return this.m_info.m_color1;
				case 2:
					return this.m_info.m_color2;
				case 3:
					return this.m_info.m_color3;
				default:
					return this.m_info.m_color0;
			}
		}
	}
}
