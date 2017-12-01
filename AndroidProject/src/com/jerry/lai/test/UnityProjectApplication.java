package com.jerry.lai.test;

import com.jerry.lai.lib.LogHelper;

import android.app.Application;
import android.content.Context;

public class UnityProjectApplication extends Application {

	@Override
	protected void attachBaseContext(Context ctx) {
		super.attachBaseContext(ctx);
		LogHelper.log("DemoApplication attachBaseContext");
	}

	@Override
	public void onCreate() {
		super.onCreate();
		LogHelper.log("DemoApplication onCreate");
	}
}