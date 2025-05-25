namespace Cezzi.Applications.Logging;

/// <summary>
/// 
/// </summary>
public class ServiceBusMonikers
{
    /// <summary>The MSG identifier</summary>
    public string MsgId => "@sb_msg_id";

    /// <summary>The subject</summary>
    public string MsgSubject => "@sb_msg_sub";

    /// <summary>The correlation identifier</summary>
    public string MsgCorrelationId => "@sb_msg_cid";

    /// <summary>The delivery count</summary>
    public string MsgDeliveryCount => "@sb_msg_dcnt";

    /// <summary>Gets the MSG entity count.</summary>
    /// <value>The MSG entity count.</value>
    public string MsgEntityCount => "@sb_msg_entitycount";

    /// <summary>Gets the topic.</summary>
    /// <value>The topic.</value>
    public string Topic => "@sb_topic";

    /// <summary>Gets the queue.</summary>
    /// <value>The queue.</value>
    public string Queue => "@sb_queue";

    /// <summary>Gets the namespace.</summary>
    /// <value>The namespace.</value>
    public string Namespace => "@sb_namespace";

    /// <summary>Gets the dead letter message identifier.</summary>
    /// <value>The dead letter message identifier.</value>
    public string DeadLetterMessageId => "@sb_dlmid";

    /// <summary>Gets the failed send message identifier.</summary>
    /// <value>The failed send message identifier.</value>
    public string FailedSendMessageId => "@sb_fsmid";
}