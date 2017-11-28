package com.jerry.lai.lib;

import android.app.Activity;
import android.content.Context;
import android.telephony.TelephonyManager;

public class UnityPluginInterface {
	private String mUnityMgr;
	private Activity mUnityActivity;

	public UnityPluginInterface(String mgr, Context unityActivity) {
		mUnityMgr = mgr;
		mUnityActivity = (Activity) unityActivity;
	}

	public String getDeviceId() {
		TelephonyManager telephonyManager = (TelephonyManager) mUnityActivity
				.getSystemService(android.content.Context.TELEPHONY_SERVICE);
		String imei = telephonyManager.getDeviceId();
		return imei;
	}
}