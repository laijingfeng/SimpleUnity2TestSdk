package com.jerry.lai.getui;

import android.content.Context;

import com.igexin.sdk.GTIntentService;
import com.igexin.sdk.message.GTCmdMessage;
import com.igexin.sdk.message.GTTransmitMessage;

public class GeTuiIntentService extends GTIntentService {
	public GeTuiIntentService() {

	}

	@Override
	public void onReceiveServicePid(Context context, int pid) {
	}

	@Override
	public void onReceiveMessageData(Context context, GTTransmitMessage msg) {
	}

	@Override
	public void onReceiveClientId(Context context, String clientid) {
		// Log.e(TAG, "onReceiveClientId -> " + "clientid = " + clientid);
	}

	@Override
	public void onReceiveOnlineState(Context context, boolean online) {
	}

	@Override
	public void onReceiveCommandResult(Context context, GTCmdMessage cmdMessage) {
	}
}