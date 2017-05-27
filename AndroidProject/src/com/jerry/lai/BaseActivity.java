package com.jerry.lai;

import android.app.Activity;

public class BaseActivity extends Activity {
	@Override
	protected void onResume() {
		super.onResume();
		JerryHelper.log("onResume");
	}

	@Override
	protected void onPause() {
		super.onPause();
		JerryHelper.log("onPause");
	}
}