package com.jerry.lai.lib;

import android.app.Activity;
import android.content.Context;
import android.telephony.TelephonyManager;

import com.jerry.lai.download.DownloadUtil;

public class UnityPluginInterface {
	private String mUnityMgr;
	private Activity mUnityActivity;
	private DownloadUtil downloadUtils = null;

	public UnityPluginInterface(String mgr, Context unityActivity) {
		mUnityMgr = mgr;
		mUnityActivity = (Activity) unityActivity;
	}

	/*
	 * 获取设备号
	 */
	public String getDeviceId() {
		TelephonyManager telephonyManager = (TelephonyManager) mUnityActivity
				.getSystemService(android.content.Context.TELEPHONY_SERVICE);
		String imei = telephonyManager.getDeviceId();
		return imei;
	}
	
	/*
	 * 下载apk
	 */
	public void downloadApk(String par) {
		downloadUtils = new DownloadUtil(mUnityActivity);
		downloadUtils.downloadApk(par);
	}
	
	public String getDownloadPro() {
		if (downloadUtils == null) {
			return null;
		}
		return downloadUtils.getDownloadPro();
	}
}