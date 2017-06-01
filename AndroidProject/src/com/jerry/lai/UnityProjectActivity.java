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
	
	public void DoHideAndroidSplash() {
		UnitySplashExtra.getInstance().hideSplash();
	}
	
	public void DoLogin() {
		//TODO
		DoLoginCallBack();
	}

	public void DoLoginCallBack() {
		//TODO
		UnityPlayer.UnitySendMessage(U3DReceiver, "DoLoginCallBack", "test");
	}

	public void DoSwitchAccount() {
	}

	@Override
	public boolean onKeyDown(int keyCode, KeyEvent event) {
		switch (keyCode) {
		case KeyEvent.KEYCODE_BACK:
			//TODO
			exitDirectly();
			break;
		default:
			break;
		}
		return super.onKeyDown(keyCode, event);
	}
	
	private void exitDirectly() {
		UnityProjectActivity.this.finish();
		android.os.Process.killProcess(android.os.Process.myPid());
	}
}
