package com.jerry.lai.lib;

import android.app.Activity;
import android.content.Context;
import android.telephony.TelephonyManager;

import com.jerry.lai.download.DownloadUtil;

public class UnityPluginInterface {
	private String mUnityMgr;
	private Activity mUnityActivity;

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
	public int downloadApk(String par) {
		return DownloadUtil.getInstance(mUnityActivity).downloadApk(par);
	}

	public String getDownloadPro() {
		return DownloadUtil.getInstance(mUnityActivity).getDownloadPro();
	}

	/*
	 * clean old version cache
	 */
	public void cleanCache() {
		if (mUnityActivity != null) {
			SpUtil.getInstance(mUnityActivity).remove(
					DownloadUtil.DOWNLOAD_SAVE_ID);
		}
	}

	public void doTest() {
		LogUtil.getInstance().log(
				"id:"
						+ SpUtil.getInstance(mUnityActivity).getLong(
								"JerrySaveDownloadId", -1L));
	}
}