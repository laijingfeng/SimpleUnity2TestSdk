package com.jerry.lai;

import android.widget.ImageView;

public class UnitySplashExtra {
	private ImageView bgView = null;
	private UnityProjectActivity mMainActivity = null;
	private static UnitySplashExtra mInstance;

	public static UnitySplashExtra getInstance() {
		if (null == mInstance) {
			synchronized (UnitySplashExtra.class) {
				if (null == mInstance) {
					mInstance = new UnitySplashExtra();
				}
			}
		}
		return mInstance;
	}

	public void onCreate(UnityProjectActivity activity) {
		mMainActivity = activity;
		showSplash();
	}

	public void showSplash() {
		if (bgView != null) {
			return;
		}
		try {
			bgView = new ImageView(mMainActivity);
			int splash_bg = mMainActivity.getResources().getIdentifier(
					"splash", "drawable", mMainActivity.getPackageName());
			bgView.setBackgroundResource(splash_bg);
			mMainActivity.getUnityPlayer().addView(bgView);
		} catch (Exception e) {
			e.printStackTrace();
		}
	}

	public void hideSplash() {
		try {
			if (bgView == null) {
				return;
			}
			mMainActivity.runOnUiThread(new Runnable() {
				public void run() {
					mMainActivity.getUnityPlayer().removeView(bgView);
					bgView = null;
				}
			});
		} catch (Exception e) {
			e.printStackTrace();
		}
	}
}