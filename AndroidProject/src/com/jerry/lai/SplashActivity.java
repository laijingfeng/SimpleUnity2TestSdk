package com.jerry.lai;

import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.os.Handler;
import android.view.Window;

public class SplashActivity extends Activity {

	private final int SPLASH_DISPLAY_LENGHT = 1000;
	private static boolean isActivity = false;

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		if (isActivity == false) {
			isActivity = true;
			getWindow().requestFeature(Window.FEATURE_NO_TITLE);
			new Handler().postDelayed(new Runnable() {
				@Override
				public void run() {
					toMainActivity();
				}
			}, SPLASH_DISPLAY_LENGHT);
		} else {
			toMainActivity();
		}
	}

	private void toMainActivity() {
		Intent intent = new Intent(SplashActivity.this,
				UnityProjectActivity.class);
		startActivity(intent);
		SplashActivity.this.finish();
		overridePendingTransition(R.anim.fade_in, R.anim.fade_out);
	}
}