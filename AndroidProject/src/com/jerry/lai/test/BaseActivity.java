package com.jerry.lai.test;

import android.app.Activity;

/*
 * 需求每一个Activity都做一些事情，则放在BaseActivity
 */
public class BaseActivity extends Activity {
	@Override
	protected void onResume() {
		super.onResume();
	}

	@Override
	protected void onPause() {
		super.onPause();
	}
}