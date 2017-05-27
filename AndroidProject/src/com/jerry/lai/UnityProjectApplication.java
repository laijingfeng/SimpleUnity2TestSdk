package com.jerry.lai;

import android.app.Application;
import android.content.Context;

public class UnityProjectApplication extends Application {

	@Override
	protected void attachBaseContext(Context ctx) {
		super.attachBaseContext(ctx);
		JerryHelper.log("DemoApplication attachBaseContext");
	}

	@Override
	public void onCreate() {
		super.onCreate();
		JerryHelper.log("DemoApplication onCreate");
	}
}