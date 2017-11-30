//var httpUrl = "http://192.168.1.160:8030";
var httpUrl = "";
var app_key = "9e304d4e8df1b74cfa009913198428ab";
var v = "v1.0";
var sign_method = "md5";
var signKey = "ab517f95fab7d57";
session_key = localStorage.getItem('session_key');
timeouts = 20000;

var gettoken="/api/Service/GetToken?staffId=10000";

//获取当前时间戳
function getTimestamp() {
	//return(Date.parse(new Date()) / 1000).toString();
	return (new Date()).valueOf();
}
//获取sign签名 
function getSign(keyOptions) {
    var sign = signKey;
    var isFirst = false;
    var str='';
	for(var key in keyOptions) {
        str += key + sign+ keyOptions[key];
	}
    //sign = sign + signKey;
    return str;
}
//获取发送数据的
function getdata(options, apiName) {
	var timestamp = getTimestamp();
	var sign = hex_md5(getSign(options));
	var data = {
		app_key: app_key,
		method: apiName,
		timestamp: timestamp,
		v: 'v1.0',
		sign_method: 'md5',
		session_key: session_key,
		sign: sign,
	};

	for(var key in options) {
		data[key] = options[key];
	}
	return data;
}
//字符串转换为十六进制
function stringToHex(str) {　　　　
	var val = "";　　　　
	for(var i = 0; i < str.length; i++) {　　　　　　
		if(val == ""){
			 val = str.charCodeAt(i).toString(16);　
		}　　　　　
		else{
			val += "," + str.charCodeAt(i).toString(16);　　　　
		}
	}　　　　
	return val;　　
}
var chars = ['0','1','2','3','4','5','6','7','8','9','A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'];

function generateMixed(n) {
     var res = "";
     for(var i = 0; i < n ; i ++) {
         var id = Math.ceil(Math.random()*35);
         res += chars[id];
     }
     return res;
}

function logData(data) {
	console.log(JSON.stringify(data));
}

/**
 * 获得存储在本地的json
 * @param {key} key : 键
 * @param {boolean} wa : 是否显示等待框
 * @param {boolean} ns : 是否不自动显示
 * @param {JSON} ws : Webview窗口属性
 */
function GetSession(key) {
	var obj = localStorage.getItem(key);
	if(obj != null) {
		return JSON.parse(obj);
	}
	return null;
}

/**
 * 获得存储在本地的json
 * @param {key} key : 键
 * @param {boolean} wa : 是否显示等待框
 * @param {boolean} ns : 是否不自动显示
 * @param {JSON} ws : Webview窗口属性
 */
function GetStorage(key) {
	var obj = storage.getItem(key);
	if(obj != null) {
		return JSON.parse(obj);
	}
	return null;
}
/**
 * 保存存储在本地的json
 * @param {key} key : 键
 * @param {options} json : 保存的参数
 */
function SetSession(key, options) {
	localStorage.setItem(key, JSON.stringify(options));
}

/**
 * 删除键值对json
 * @param {key} key : 键
 */
function SessionRemove(key) {
	localStorage.removeItem(key);
}

/**
 * 保存存储在本地的json
 * @param {key} key : 键
 * @param {options} json : 保存的参数
 */
function SetStorage(key, options) {
	storage.setItem(key, JSON.stringify(options));
}

(function(w) {
	//公共ajax方法
	w.baseAjax = function(url, options, callFun) {
		//var data = getdata(options, 'com.feiyit.yl.gcomment');
		//console.log(logData(options));
		mui.ajax(httpUrl + url, {
			data: options,
			async: false,
			dataType: 'json', //服务器返回json格式数据
			type: 'post', //HTTP请求类型
			timeout: timeouts, //超时时间设置为10秒；
			success: function(data) {
				callFun(data);
			},
			error: function(xhr, type, errorThrown) {
				if(type == 'timeout') {
					mui.toast('连接超时，请稍后重试！');
				} else if(type == 'error') {
					mui.toast('连接异常，请稍后重试！');
				}
			}
		});
	};
})(window);

function diaplayTime(data) {
	var str = data;
	//将字符串转换成时间格式
	var timePublish = new Date(str);
	var timeNow = new Date();
	var minute = 1000 * 60;
	var hour = minute * 60;
	var day = hour * 24;
	var month = day * 30;
	var diffValue = timeNow - timePublish;
	var diffMonth = diffValue / month;
	var diffWeek = diffValue / (7 * day);
	var diffDay = diffValue / day;
	var diffHour = diffValue / hour;
	var diffMinute = diffValue / minute;

	if(diffValue < 0) {
		return "刚刚发表"; //alert("错误时间");
	} else if(diffMonth > 3) {
		result = timePublish.getFullYear() + "-";
		result += timePublish.getMonth() + "-";
		result += timePublish.getDate();
	} else if(diffMonth > 1) {
		result = parseInt(diffMonth) + "月前";
	} else if(diffWeek > 1) {
		result = parseInt(diffWeek) + "周前";
	} else if(diffDay > 1) {
		result = parseInt(diffDay) + "天前";
	} else if(diffHour > 1) {
		result = parseInt(diffHour) + "小时前";
	} else if(diffMinute > 1) {
		result = parseInt(diffMinute) + "分钟前";
	} else {
		result = "刚刚发表";
	}
	return result;
}