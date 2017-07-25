package com.jerry.lai.test;

import android.os.Bundle;
import android.view.KeyEvent;

import com.google.gson.Gson;
import com.unity3d.player.UnityPlayer;
import com.unity3d.player.UnityPlayerActivity;

public class UnityProjectActivity extends UnityPlayerActivity {

	private static String U3DReceiver = "SDKManager";

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		UnitySplashExtra.getInstance().onCreate(this);
	};

	public UnityPlayer getUnityPlayer() {
		return mUnityPlayer;
	}

	// region ToUnity

	/*
	 * 隐藏遮盖闪图
	 */
	public void DoHideAndroidSplash() {
		UnitySplashExtra.getInstance().hideSplash();
	}

	/*
	 * 登录
	 */
	public void DoLogin() {
		DoLoginCallback();
	}

	/*
	 * 切换帐号
	 */
	public void DoSwitchAccount() {
		DoSwitchAccountCallback();
	}

	/*
	 * 登录回调
	 */
	public void DoLoginCallback() {
		LoginCallbackData loginData = new LoginCallbackData();
		loginData.uid = "lai123";
		loginData.token = "test2017";
		UnityPlayer.UnitySendMessage(U3DReceiver, "DoLoginCallback",
				new Gson().toJson(loginData));
	}

	/*
	 * 切换帐号回调
	 */
	public void DoSwitchAccountCallback() {
		UnityPlayer.UnitySendMessage(U3DReceiver, "DoSwitchAccountCallback",
				"switch");
	}

	/*
	 * 退出
	 */
	public void DoExit() {
		UnityPlayer.UnitySendMessage(U3DReceiver, "DoExitFromSDK", "exit");
	}

	@Override
	public boolean onKeyDown(int keyCode, KeyEvent event) {
		switch (keyCode) {
		case KeyEvent.KEYCODE_BACK:
			exitDirectly();
			break;
		default:
			break;
		}
		return super.onKeyDown(keyCode, event);
	}

	private void exitDirectly() {
		DoExit();
		UnityProjectActivity.this.finish();
		android.os.Process.killProcess(android.os.Process.myPid());
	}

	// endregion ToUnity
}
