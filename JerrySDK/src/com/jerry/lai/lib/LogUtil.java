package com.jerry.lai.lib;

import android.util.Log;

/**
 * 辅助Log
 * @author LaiJingfeng
 *
 */
public class LogUtil {
	private static final String TAG = "Jerry";
	private static LogUtil mInstance;
	private boolean isShowLog = false;

	public static LogUtil getInstance() {
		if (null == mInstance) {
			synchronized (LogUtil.class) {
				if (null == mInstance) {
					mInstance = new LogUtil();
				}
			}
		}
		return mInstance;
	}

	/**
	 * log
	 * @param msg
	 */
	public void log(String msg) {
		if (isShowLog) {
			Log.d(TAG, msg);
		}
	}
}