#region License
/*
 * Bubble Engine
 * This file is licensed under the MIT License. See LICENSE for Details
 */
#endregion
using System;
using System.Collections.Concurrent;
using System.Threading;
namespace BubbleEngine
{
	/// <summary>
	/// Allows for pushing actions onto the UI thread
	/// </summary>
	public static class Threading
	{
		static int uiID = -1;
		static ConcurrentQueue<Action> actions = new ConcurrentQueue<Action>();

		public static void EnsureUIThread(Action action)
		{
			if (uiID == -1)
				throw new Exception ("No UI thread");
			if (Thread.CurrentThread.ManagedThreadId == uiID) {
				action ();
			} else {
				bool completed = false;
				actions.Enqueue (delegate {
					action();
					completed = true;
				});
				//wait for UI thread to process
				while (!completed) {
					Thread.Sleep (0);
				}
			}
		}

		internal static void RegisterUIThread()
		{
			uiID = Thread.CurrentThread.ManagedThreadId;
		}

		internal static void Update()
		{
			if (actions.Count > 0) {
				Action result;
				if (actions.TryDequeue (out result))
					result ();
			}
		}
	}
}

