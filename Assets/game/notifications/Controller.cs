using UnityEngine;
using System.Collections;

public interface Controller
{
	void OnNotification (string p_event_path, Object p_target, params object[] p_data);
}
