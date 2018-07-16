﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrafficManager.Traffic.Enums;

namespace TrafficManager.Traffic.Data {
	public struct PathUnitQueueItem {
		public uint nextPathUnitId; // access requires acquisition of CustomPathFind.QueueLock
		public ExtVehicleType vehicleType; // access requires acquisition of m_bufferLock
		public ExtPathType pathType; // access requires acquisition of m_bufferLock
		public ushort vehicleId; // access requires acquisition of m_bufferLock
		public bool queued; // access requires acquisition of m_bufferLock

		//public void Reset() {
		//	vehicleType = ExtVehicleType.None;
		//	pathType = ExtPathType.None;
		//	vehicleId = 0;
		//}

		public override string ToString() {
			return $"[PathUnitQueueItem\n" +
			"\t" + $"nextPathUnitId={nextPathUnitId}\n" +
			"\t" + $"vehicleType={vehicleType}\n" +
			"\t" + $"pathType={pathType}\n" +
			"\t" + $"vehicleId={vehicleId}\n" +
			"\t" + $"queued={queued}\n" +
			"PathUnitQueueItem]";
		}
	}
}
