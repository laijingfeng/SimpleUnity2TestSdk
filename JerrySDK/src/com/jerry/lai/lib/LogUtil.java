package com.jerry.lai.lib;

import android.util.Log;

/*
 * 辅助Log
 */
public class LogUtil {
	private static final String TAG = "Jerry";
	private static LogUtil mInstance;
	private boolean isShowLog = true;

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

	/*
	 * log
	 */
	public void log(String msg) {
		if (isShowLog) {
			Log.d(TAG, msg);
		}
	}
}