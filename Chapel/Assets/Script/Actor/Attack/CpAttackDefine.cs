using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Assertions;
using TMPro;

public class CpAttackDefine
{

}

// UŒ‚‚ð—^‚¦‚éŽÒ/Žó‚¯‚éŽÒ‚Æ‚µ‚Ä‚Ý‚½‚Æ‚«‚Ì‹æ•ª
public enum ECpAttackSenderGroup
{
    None = -1,
    Player = 0,
    PlayerShot = 1,
    Enemy = 100,
    EnemyShot = 101,
}

public enum ECpAttackReceiverGroup
{
    None = -1,
    Player = 0,
    PlayerShot = 1,
    Enemy = 100,
    EnemyShot = 101,
}

[System.Serializable]
public class CpAttackSenderParamElement
{
    public string Title => SltEnumUtil.ToString(Sender);
    public bool CanAttack(ECpAttackReceiverGroup receiver)
    {
        return AttackableReceivers.Contains(receiver);
    }

    public ECpAttackSenderGroup Sender;
    [SerializeField]
    List<ECpAttackReceiverGroup> AttackableReceivers = new List<ECpAttackReceiverGroup>();
}

[System.Serializable]
public class CpAttackSenderParam
{
    public bool CanAttack(ECpAttackSenderGroup sender, ECpAttackReceiverGroup receiver)
    {
        CpAttackSenderParamElement element = FindElement(sender);
        return element.CanAttack(receiver);
    }

    CpAttackSenderParamElement FindElement(ECpAttackSenderGroup sender)
    {
        for (int index = 0; index < _attackSenderParamList.Count; index++)
        {
            if (_attackSenderParamList[index].Sender == sender)
            {
                return _attackSenderParamList[index];
            }
        }
        Assert.IsTrue(false, sender + "‚ªAttackSenderParam()‚ÉŠÜ‚Ü‚ê‚Ä‚¢‚Ü‚¹‚ñ");
        return null;
    }

    [SerializeField]
    [ListDrawerSettings(ListElementLabelName = "Title")]
    List<CpAttackSenderParamElement> _attackSenderParamList = new List<CpAttackSenderParamElement>();
}

public static class CpAttackUtil
{
    public static void OnTriggerEnter2D(ICpAttackSendable sender, ICpAttackReceivable receiver, Collider2D receiverCollision)
    {
        if (sender == null || receiver == null || receiverCollision == null)
        {
            return;
        }
        ECpAttackSenderGroup senderGroup = sender.GetAttackSenderGroup();
        ECpAttackReceiverGroup receiverGroup = receiver.GetAttackReceiverGroup();

        if (!CanAttack(senderGroup, receiverGroup))
        {
            return;
        }

        FCpAttackSendParam attackSendParam = sender.CreateAttackSendParam();
        receiver.OnReceiveAttack(attackSendParam);
        sender.OnSendAttack();
    }

    public static bool CanAttack(ECpAttackSenderGroup sender, ECpAttackReceiverGroup receiver)
    {
        bool bAttackable = CpAttackSenderParamScriptableObject.Get().AttackSenderParam.CanAttack(sender, receiver);

        return bAttackable;
    }
}