﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrafficManager.Traffic.Data;
using TrafficManager.Traffic.Enums;
using UnityEngine;

namespace TrafficManager.Manager {
	public interface IExtNodeManager {
		/// <summary>
		/// Extended node data
		/// </summary>
		ExtNode[] ExtNodes { get; }

		/// <summary>
		/// Checks if the node with the given id is valid.
		/// </summary>
		/// <param name="nodeId">node id</param>
		/// <returns><code>true</code> if the node is valid, <code>false</code> otherwise</returns>
		bool IsValid(ushort nodeId);

		/// <summary>
		/// Adds the segment to the given node
		/// </summary>
		/// <param name="nodeId">node id</param>
		/// <param name="segmentId">segment id</param>
		void AddSegment(ushort nodeId, ushort segmentId);

		/// <summary>
		/// Removes the segment from the given node
		/// </summary>
		/// <param name="nodeId">node id</param>
		/// <param name="segmentId">segment id</param>
		void RemoveSegment(ushort nodeId, ushort segmentId);
	}
}
