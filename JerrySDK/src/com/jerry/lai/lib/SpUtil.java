package com.jerry.lai.lib;

import android.content.Context;
import android.content.SharedPreferences;

public class SpUtil {
	private SharedPreferences sp;
	private static SpUtil instance;

	private SpUtil(Context context) {
		sp = context.getSharedPreferences("JerrySdkSp", Context.MODE_PRIVATE);
	}

	public static SpUtil getInstance(Context context) {
		if (instance == null) {
			synchronized (SpUtil.class) {
				instance = new SpUtil(context.getApplicationContext());
			}
		}
		return instance;
	}

	public SpUtil putInt(String key, int value) {
		sp.edit().putInt(key, value).apply();
		return this;
	}

	public int getInt(String key, int dValue) {
		return sp.getInt(key, dValue);
	}

	public SpUtil putLong(String key, long value) {
		sp.edit().putLong(key, value).apply();
		return this;
	}

	public long getLong(String key, Long dValue) {
		return sp.getLong(key, dValue);
	}

	public SpUtil putFloat(String key, float value) {
		sp.edit().putFloat(key, value).apply();
		return this;
	}

	public Float getFloat(String key, Float dValue) {
		return sp.getFloat(key, dValue);
	}

	public SpUtil putBoolean(String key, boolean value) {
		sp.edit().putBoolean(key, value).apply();
		return this;
	}

	public Boolean getBoolean(String key, boolean dValue) {
		return sp.getBoolean(key, dValue);
	}

	public SpUtil putString(String key, String value) {
		sp.edit().putString(key, value).apply();
		return this;
	}

	public String getString(String key, String dValue) {
		return sp.getString(key, dValue);
	}

	public void remove(String key) {
		if (isExist(key)) {
			SharedPreferences.Editor editor = sp.edit();
			editor.remove(key);
			editor.apply();
		}
	}

	public boolean isExist(String key) {
		return sp.contains(key);
	}
}
