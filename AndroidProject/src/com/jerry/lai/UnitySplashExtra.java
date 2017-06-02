package com.jerry.lai;

import android.os.Handler;
import android.widget.ImageView;

public class UnitySplashExtra {
	private ImageView bgView = null;
	private boolean isFinish = false;
	private final int SPLASH_DISPLAY_LENGHT = 1000;
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
		isFinish = false;
		mMainActivity = activity;
		showSplash();
	}

	public void showSplash() {
		if (bgView != null) {
			return;
		}
		setBgView("splash");
		new Handler().postDelayed(new Runnable() {
			@Override
			public void run() {
				if (isFinish == false) {
					setBgView("splash_unity");
				}
			}
		}, SPLASH_DISPLAY_LENGHT);
	}

	private void setBgView(String image_name) {
		if (isFinish) {
			return;
		}
		try {
			boolean first = false;
			if (bgView == null) {
				first = true;
				bgView = new ImageView(mMainActivity);
			}

			int splash_bg = mMainActivity.getResources().getIdentifier(
					image_name, "drawable", mMainActivity.getPackageName());
			bgView.setBackgroundResource(splash_bg);

			if (first) {
				mMainActivity.getUnityPlayer().addView(bgView);
			}
		} catch (Exception e) {
			e.printStackTrace();
		}
	}

	public void hideSplash() {
		isFinish = true;
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