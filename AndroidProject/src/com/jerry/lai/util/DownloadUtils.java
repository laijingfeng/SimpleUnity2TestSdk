package com.jerry.lai.util;

import com.google.gson.Gson;

import android.app.DownloadManager;
import android.app.DownloadManager.Query;
import android.app.DownloadManager.Request;
import android.app.ProgressDialog;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.database.Cursor;
import android.net.Uri;
import android.os.Environment;
import android.widget.Toast;

public class DownloadUtils {
	private DownloadManager downloadManager;
	private Context mContext;
	private long downloadId;
	private Query dowloadQuery = null;

	public DownloadUtils(Context context) {
		this.mContext = context;
	}

	public String getDownloadPro() {
		DownloadPro pro = new DownloadPro();
		if (dowloadQuery != null && downloadManager != null) {
			Cursor c = downloadManager.query(dowloadQuery);
			if (c.moveToFirst()) {
				pro.loadedSize = c
						.getInt(c
								.getColumnIndex(DownloadManager.COLUMN_BYTES_DOWNLOADED_SO_FAR));
				pro.totalSize = c
						.getInt(c
								.getColumnIndex(DownloadManager.COLUMN_TOTAL_SIZE_BYTES));
			}
			c.close();
		}
		return new Gson().toJson(pro);
	}

	public void downloadAPK(String url, String name) {
		Request request = new Request(Uri.parse(url));
		request.setAllowedOverRoaming(false);
		request.setNotificationVisibility(DownloadManager.Request.VISIBILITY_VISIBLE);
		request.setTitle("下载");
		request.setDescription("正在下载");
		request.setVisibleInDownloadsUi(true);

		request.setDestinationInExternalPublicDir(
				Environment.DIRECTORY_DOWNLOADS, name);

		downloadManager = (DownloadManager) mContext
				.getSystemService(Context.DOWNLOAD_SERVICE);
		downloadId = downloadManager.enqueue(request);
		dowloadQuery = new Query();
		dowloadQuery.setFilterById(downloadId);
		mContext.registerReceiver(receiver, new IntentFilter(
				DownloadManager.ACTION_DOWNLOAD_COMPLETE));
	}

	private BroadcastReceiver receiver = new BroadcastReceiver() {
		@Override
		public void onReceive(Context context, Intent intent) {
			checkStatus();
		}
	};

	private void checkStatus() {
		Cursor c = downloadManager.query(dowloadQuery);
		if (c.moveToFirst()) {
			int status = c.getInt(c
					.getColumnIndex(DownloadManager.COLUMN_STATUS));
			switch (status) {
			case DownloadManager.STATUS_PAUSED:
				break;
			case DownloadManager.STATUS_PENDING:
				break;
			case DownloadManager.STATUS_RUNNING:
				break;
			case DownloadManager.STATUS_SUCCESSFUL:
				installAPK();
				break;
			case DownloadManager.STATUS_FAILED:
				Toast.makeText(mContext, "下载失败", Toast.LENGTH_SHORT).show();
				break;
			}
		}
		c.close();
	}

	private void installAPK() {
		Uri downloadFileUri = downloadManager
				.getUriForDownloadedFile(downloadId);
		if (downloadFileUri != null) {
			Intent intent = new Intent(Intent.ACTION_VIEW);
			intent.setDataAndType(downloadFileUri,
					"application/vnd.android.package-archive");
			intent.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
			mContext.startActivity(intent);
			mContext.unregisterReceiver(receiver);
		}
	}
}