package com.jerry.lai.download;

import java.io.File;

import android.app.DownloadManager;
import android.app.DownloadManager.Request;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.content.pm.PackageManager;
import android.database.Cursor;
import android.net.Uri;
import android.os.Environment;
import android.widget.Toast;

import com.google.gson.Gson;
import com.jerry.lai.lib.SpUtil;

public class DownloadUtil {
	private DownloadManager mDownloadManager = null;
	private Context mContext = null;
	public static final String DOWNLOAD_SAVE_ID = "JerrySaveDownloadId";

	/*
	 * now download id
	 */
	private long mDownloadId;
	/*
	 * now download parameter
	 */
	private DownloadPar downloadPar = null;
	/*
	 * is now download finish
	 */
	private Boolean downloadFinish = false;
	/*
	 * instance
	 */
	private static DownloadUtil mInstance;

	public static DownloadUtil getInstance(Context context) {
		if (mInstance == null) {
			synchronized (DownloadUtil.class) {
				mInstance = new DownloadUtil(context);
			}
		}
		return mInstance;
	}

	public DownloadUtil(Context context) {
		this.mContext = context;
	}

	/*
	 * DownloadManager
	 */
	private DownloadManager DM() {
		if (mDownloadManager == null && mContext != null) {
			mDownloadManager = (DownloadManager) mContext
					.getSystemService(Context.DOWNLOAD_SERVICE);
		}
		return mDownloadManager;
	}

	private int getDownloadStatus(long downloadId) {
		DownloadManager.Query query = new DownloadManager.Query()
				.setFilterById(downloadId);
		Cursor c = DM().query(query);
		if (c != null) {
			try {
				if (c.moveToFirst()) {
					return c.getInt(c
							.getColumnIndexOrThrow(DownloadManager.COLUMN_STATUS));
				}
			} finally {
				c.close();
			}
		}
		return -1;
	}

	public String getDownloadPro() {
		DownloadPro pro = new DownloadPro();
		if (downloadFinish) {
			pro.finish = true;
		} else if (DM() != null) {
			DownloadManager.Query query = new DownloadManager.Query()
					.setFilterById(mDownloadId);
			Cursor c = DM().query(query);
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

	/*
	 * -1参数异常；-2禁用了下载；0正常；1下载完成，进入安装
	 */
	public int downloadApk(String par) {
		downloadFinish = false;
		downloadPar = new Gson().fromJson(par, DownloadPar.class);
		if (downloadPar == null) {
			return -1;
		}
		if (!canDownloadState()) {
			return -2;
		}

		mDownloadId = SpUtil.getInstance(mContext).getLong(DOWNLOAD_SAVE_ID,
				-1L);
		if (mDownloadId != -1L) {
			int status = getDownloadStatus(mDownloadId);
			if (status == DownloadManager.STATUS_SUCCESSFUL) {
				String apkFile = getDownloadPath();
				if (apkFile != null && new File(apkFile).exists()) {
					installAPK();
					return 1;
				} else {
					SpUtil.getInstance(mContext).remove(DOWNLOAD_SAVE_ID);
					newDownload();
				}
			} else if (status == DownloadManager.STATUS_RUNNING) {
				// downloading
			} else {
				SpUtil.getInstance(mContext).remove(DOWNLOAD_SAVE_ID);
				newDownload();
			}
		} else {
			newDownload();
		}
		return 0;
	}

	private void newDownload() {
		Request request = new Request(Uri.parse(downloadPar.url));
		request.setAllowedOverRoaming(true);
		request.setNotificationVisibility(DownloadManager.Request.VISIBILITY_VISIBLE);
		request.setTitle(downloadPar.noticeShowName);
		request.setDescription(downloadPar.noticeShowName);
		request.setVisibleInDownloadsUi(true);
		request.setDestinationInExternalFilesDir(mContext,
				Environment.DIRECTORY_DOWNLOADS, downloadPar.apkName + ".apk");

		mDownloadId = DM().enqueue(request);
		SpUtil.getInstance(mContext).putLong(DOWNLOAD_SAVE_ID, mDownloadId);

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
		DownloadManager.Query query = new DownloadManager.Query()
				.setFilterById(mDownloadId);
		Cursor c = DM().query(query);
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
		downloadFinish = true;
		Uri downloadFileUri = getDownloadUri();
		if (downloadFileUri != null) {
			Intent intent = new Intent(Intent.ACTION_VIEW);
			intent.setDataAndType(downloadFileUri,
					"application/vnd.android.package-archive");
			intent.addFlags(Intent.FLAG_GRANT_READ_URI_PERMISSION);
			intent.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
			mContext.startActivity(intent);
			mContext.unregisterReceiver(receiver);
		}
		// notice : can not remove saved is here, because use can cancel install
	}

	private Uri getDownloadUri() {
		String path = getDownloadPath();
		if (path != null) {
			return Uri.parse("file://" + path);
		}
		return null;
	}

	private String getDownloadPath() {
		if (downloadPar == null) {
			return null;
		}
		File file = mContext
				.getExternalFilesDir(Environment.DIRECTORY_DOWNLOADS);
		if (file.exists()) {
			return file.getAbsolutePath() + "/" + downloadPar.apkName + ".apk";
		}
		return null;
	}

	/*
	 * 是否可以下载
	 */
	private boolean canDownloadState() {
		try {
			int state = mContext.getPackageManager()
					.getApplicationEnabledSetting(
							"com.android.providers.downloads");
			if (state == PackageManager.COMPONENT_ENABLED_STATE_DISABLED
					|| state == PackageManager.COMPONENT_ENABLED_STATE_DISABLED_USER
					|| state == PackageManager.COMPONENT_ENABLED_STATE_DISABLED_UNTIL_USED) {
				return false;
			}

		} catch (Exception e) {
			e.printStackTrace();
			return false;
		}
		return true;
	}
}