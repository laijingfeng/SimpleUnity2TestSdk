package com.jerry.lai.lib;

import android.app.Activity;
import android.content.ClipData;
import android.content.ClipDescription;
import android.content.ClipboardManager;
import android.content.Context;
import android.os.Handler;
import android.os.Looper;
import android.telephony.TelephonyManager;

import com.jerry.lai.download.DownloadUtil;
import com.unity3d.player.UnityPlayer;

public class UnityPluginInterface {
	private static String mUnityMgr;
	private Activity mUnityActivity = null;
	private static ClipboardManager mClipboardMgr = null;

	public UnityPluginInterface(String mgr, Context unityActivity) {
		mUnityMgr = mgr;
		mUnityActivity = (Activity) unityActivity;
	}

	/**
	 * 获取设备号
	 * 
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
	 * 
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
	
	/*
	 * JustTest
	 */
	public void doTest() {
		LogUtil.getInstance().log(
				"id:"
						+ SpUtil.getInstance(mUnityActivity).getLong(
								"JerrySaveDownloadId", -1L));
	}
	
	/*
	 * Copy Text to Clipboard 
	 */
	public void copyTextToClipboard(String str) throws Exception {
		if (Looper.myLooper() == null) {
			Looper.prepare();
		}
		Handler handler = new Handler();
		mClipboardMgr = (ClipboardManager) mUnityActivity
				.getSystemService(Activity.CLIPBOARD_SERVICE);
		ClipData textCd = ClipData.newPlainText("data", str);
		mClipboardMgr.setPrimaryClip(textCd);
		handler.getLooper().quit();
	}

	/*
	 * Get Text From Clipboard
	 */
	public String getTextFromClipboard() {
		if (mClipboardMgr != null
				&& mClipboardMgr.hasPrimaryClip()
				&& mClipboardMgr.getPrimaryClipDescription().hasMimeType(
						ClipDescription.MIMETYPE_TEXT_PLAIN)) {
			ClipData cdText = mClipboardMgr.getPrimaryClip();
			ClipData.Item item = cdText.getItemAt(0);
			return item.getText().toString();
		}
		return "";
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
}