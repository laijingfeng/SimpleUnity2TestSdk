package com.jerry.lai;

import android.os.Bundle;
import android.view.KeyEvent;

import com.unity3d.player.UnityPlayer;
import com.unity3d.player.UnityPlayerActivity;

public class UnityProjectActivity extends UnityPlayerActivity {

	private static String U3DReceiver = "Manager";
	
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		UnitySplashExtra.getInstance().onCreate(this);
	};

	public UnityPlayer getUnityPlayer() {
		return mUnityPlayer;
	}
	
	// region ToUnity 
	
	public void DoHideAndroidSplash() {
		UnitySplashExtra.getInstance().hideSplash();
	}
	
	public void DoLogin() {
		DoLoginCallback();
	}
	
	public void DoSwitchAccount() {
		DoSwitchAccountCallback();
	}
	
	public void DoLoginCallback() {
		UnityPlayer.UnitySendMessage(U3DReceiver, "DoLoginCallback", "login");
	}
	
	public void DoSwitchAccountCallback() {
		UnityPlayer.UnitySendMessage(U3DReceiver, "DoSwitchAccountCallback", "switch");
	}
	
	public void DoExit() {
		UnityPlayer.UnitySendMessage(U3DReceiver, "DoExit", "exit");
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
