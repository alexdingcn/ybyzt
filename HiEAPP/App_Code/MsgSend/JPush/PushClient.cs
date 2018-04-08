using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


internal class PushClient : BaseHttpClient
{
    private const String HOST_NAME_SSL = "https://api.jpush.cn";
    private const String PUSH_PATH = "/v3/push";

    private String appKey;
    private String masterSecret;

    public PushClient(String appKey, String masterSecret)
    {
        this.appKey = appKey;
        this.masterSecret = masterSecret;
    }

    public MessageResult sendPush(PushPayload payload)
    {
        Preconditions.checkArgument(payload != null, "pushPayload should not be empty");
        payload.Check();
        String payloadJson = payload.ToJson();
        return sendPush(payloadJson);
    }

    public MessageResult sendPush(string payloadString)
    {
        Preconditions.checkArgument(!string.IsNullOrEmpty(payloadString), "payloadString should not be empty");

        String url = HOST_NAME_SSL;
        url += PUSH_PATH;               //"https://api.jpush.cn/v3/push";
        ResponseWrapper result = sendPost(url, Authorization(), payloadString);
        MessageResult messResult = new MessageResult();
        messResult.ResponseResult = result;

        JpushSuccess jpushSuccess = JsonConvert.DeserializeObject<JpushSuccess>(result.responseContent);
        if (!string.IsNullOrEmpty(jpushSuccess.sendno))
            messResult.sendno = long.Parse(jpushSuccess.sendno);
        if (!string.IsNullOrEmpty(jpushSuccess.msg_id))
            messResult.msg_id = long.Parse(jpushSuccess.msg_id);

        return messResult;
    }

    private String Authorization()
    {

        Debug.Assert(!string.IsNullOrEmpty(this.appKey));
        Debug.Assert(!string.IsNullOrEmpty(this.masterSecret));

        String origin = this.appKey + ":" + this.masterSecret;
        return Base64.getBase64Encode(origin);
    }
}

internal enum MsgTypeEnum
{
    NOTIFICATIFY = 1,
    COUSTOM_MESSAGE = 2
}

