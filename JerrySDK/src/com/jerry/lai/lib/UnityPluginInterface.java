package com.jerry.lai.lib;

import android.app.Activity;
import android.content.Context;
import android.telephony.TelephonyManager;

import com.jerry.lai.download.DownloadUtil;
import com.unity3d.player.UnityPlayer;

public class UnityPluginInterface {
	private static String mUnityMgr;
	private Activity mUnityActivity;

	public UnityPluginInterface(String mgr, Context unityActivity) {
		mUnityMgr = mgr;
		mUnityActivity = (Activity) unityActivity;
	}

	/**
	 * 获取设备号
	 * @return
	 */
	public String getDeviceId() {
		TelephonyManager telephonyManager = (TelephonyManager) mUnityActivity
				.getSystemService(android.content.Context.TELEPHONY_SERVICE);
		String imei = telephonyManager.getDeviceId();
		return imei;
	}

	/**
	 * 下载apk
	 * @param par
	 * @return
	 */
	public int downloadApk(String par) {
		return DownloadUtil.getInstance(mUnityActivity).downloadApk(par);
	}

	public String getDownloadPro() {
		return DownloadUtil.getInstance(mUnityActivity).getDownloadPro();
	}

	/**
	 * clean old version cache
	 */
	public void cleanCache() {
		if (mUnityActivity != null) {
			SpUtil.getInstance(mUnityActivity).remove(
					DownloadUtil.DOWNLOAD_SAVE_ID);
		}
	}

	/**
	 * SendMsg2Unity
	 * 
	 * @param funcName
	 * @param parameter
	 *            notice : can not be null
	 */
	public static void SendMsg2Unity(String funcName, String parameter) {
		UnityPlayer.UnitySendMessage(mUnityMgr, funcName, parameter);
	}

	public void doTest() {
		LogUtil.getInstance().log(
				"id:"
						+ SpUtil.getInstance(mUnityActivity).getLong(
								"JerrySaveDownloadId", -1L));
	}
}