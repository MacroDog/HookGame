//
// EventEnum.cs
//
// Author:
// [ChenJiasheng]
//
// Copyright (C) 2014 Nanjing Xiaoxi Network Technology Co., Ltd. (http://www.mogoomobile.com)
using UnityEngine;
using System;
using System.Collections;

namespace GameClient {
	/// <summary>
	/// 事件枚举
	/// </summary>
	public enum EventEnum {
		None,

		/// <summary>
		/// 表示所有事件，注册后可得到所有事件的通知<para/>
		/// 参数：具体事件的参数
		/// </summary>
		AllEvent,

		#region SystemEvent
		/// <summary>
		/// 登录<para/>
		/// 参数：无
		/// </summary>
		System_Login,

		/// <summary>
		/// 注销<para/>
		/// 参数：无
		/// </summary>
		System_Logout,

		/// <summary>
		/// 客户端初始化完成<para/>
		/// 参数：无
		/// </summary>
		System_ClientInited,

		/// <summary>
		/// WebSocket客户端已连接<para/>
		/// 参数：无
		/// </summary>
		System_WsClientConnected,

		/// <summary>
		/// WebSocket客户端已断开<para/>
		/// 参数：无
		/// </summary>
		System_WsClientClosed,

		/// <summary>
		/// 数据包请求
		/// </summary>
		System_PackOnRequest,

		/// <summary>
		/// 数据包响应
		/// </summary>
		System_PackOnResponse,

		/// <summary>
		/// 登陆后进入游戏<para/>
		/// 参数：无
		/// </summary>
		System_EnterGame,

		/// <summary>
		/// 跨天<para/>
		/// 参数：无
		/// </summary>
		System_OverDay,

		/// <summary>
		/// 新功能解锁<para/>
		/// 参数：SystemFunc
		/// </summary>
		System_NewFuncUnlocked,

		/// <summary>
		/// 充值完成<para/>
		/// 参数：订单ID
		/// </summary>
		System_OrderPaied,

		/// <summary>
		/// 活动购买<para/>
		/// 参数：订单ID，数量
		/// </summary>
		System_ActivityPaied,

		/// <summary>
		/// 使用兑换码<para/>
		/// 参数：无
		/// </summary>
		System_UseRewardCode,

        /// <summary>
        /// 活动查询完毕
        /// </summary>
        System_ActivityInited,

        /// <summary>
        /// 开服狂欢活动查询完毕
        /// </summary>
        System_CarnivalInited,

		/// <summary>
		/// 限时英雄查询完毕
		/// </summary>
		System_LimitHeroInited,

		/// <summary>
		/// 刷新签到表
		/// </summary>
		System_RefreshSignIn,

		#endregion

		#region Game

		/// <summary>
		/// 开始游戏<para/>
		/// 参数：模式，关卡ID
		/// </summary>
		GameScene_Start,

		/// <summary>
		/// 结束游戏<para/>
		/// 参数：模式，关卡ID，通关星级
		/// </summary>
		GameScene_End,

		/// <summary>
		/// 中途退出游戏<para/>
		/// 参数：模式，关卡ID，通关星级(0)
		/// </summary>
		GameScene_Abort,

		/// <summary>
		/// 开始某波<para/>
		/// 参数：模式，关卡ID，波数ID
		/// </summary>
		GameScene_StartWave,

		/// <summary>
		/// 结束某波<para/>
		/// 参数：模式，关卡ID，波数ID
		/// </summary>
		GameScene_EndWave,

		/// <summary>
		/// 创建怪物<para/>
		/// 参数：模式，关卡ID，批次，怪物列表
		/// </summary>
		GameScene_CreateMonsters,

		/// <summary>
		/// 计算掉落产出<para/>
		/// 参数：MapInfo，List[ItemData]
		/// </summary>
		GameScene_CalcDrop,

		/// <summary>
		/// 击杀敌人<para/>
		/// 参数：MapInfo，EnemyDef，GameReward
		/// </summary>
		GameScene_KillEnemy,

		#endregion

		#region UI

		/// <summary>
		/// 进入某界面<para/>
		/// 参数：界面类型
		/// </summary>
		UI_EnterMenu,

		/// <summary>
		/// 进入某对话框<para/>
		/// 参数：对话框类型
		/// </summary>
		UI_EnterDialog,

		/// <summary>
		/// 游戏胜利并显示结算界面<para/>
		/// 参数：模式，关卡ID，通关星级
		/// </summary>
		UI_GameOver_Win,

		/// <summary>
		/// 游戏失败并显示结算界面<para/>
		/// 参数：模式，关卡ID，通关星级
		/// </summary>
		UI_GameOver_Fail,

		/// <summary>
		/// 结算界面宝箱打开<para/>
		/// 参数：模式，关卡ID，通关星级
		/// </summary>
		UI_TreasureOpen,

        /// <summary>
        /// 主界面按钮开关
        /// 参数：开，关
        /// </summary>
        UI_OpenTopRightButton,

		/// <summary>
		/// 返回主城
		/// </summary>
		UI_BackMain,

		#endregion

		#region Player

		/// <summary>
		/// 玩家升级<para/>
		/// 参数：原等级，新等级
		/// </summary>
		Player_LevelUp,

		/// <summary>
		/// 获得月卡<para/>
		/// 参数：原剩余天数，新剩余天数
		/// </summary>
		Player_GetMonthCard,

        /// <summary>
		/// 获得至尊卡<para/>
		/// 参数：原剩余天数，新剩余天数
		/// </summary>
		Player_GetExtremeCard,

        /// <summary>
        /// VIP升级<para/>
        /// 参数：原等级，新等级
        /// </summary>
        Player_VIPLevelUp,

		/// <summary>
		/// 更改玩家昵称<para/>
		/// 参数：是否使用元宝
		/// </summary>
		Player_ChangeName,

        /// <summary>
        /// 玩家满级
        /// </summary>
        Player_FullLevel,

		/// <summary>
		/// 更改玩家头像<para/>
		/// 参数：头像ID
		/// </summary>
		Player_ChangeAvatar,

		/// <summary>
		/// 体力值数量变化<para/>
		/// 参数：原数量，新数量
		/// </summary>
		Player_StaminaChange,

		/// <summary>
		/// 购买体力<para/>
		/// 参数：无
		/// </summary>
		Player_BuyStamina,

		/// <summary>
		/// 签到<para/>
		/// 参数：普通签，VIP签
		/// </summary>
		Player_SignIn,

		/// <summary>
		/// 使用炼金术<para/>
		/// 参数：次数
		/// </summary>
		Player_UseAlchemy,

        /// <summary>
		/// 获得体力卡<para/>
		/// 参数：原剩余天数，新剩余天数
		/// </summary>
        Player_GetStaminaCard,

        #endregion

        #region Campaign

        /// <summary>
        /// 剧情关卡结束
        /// </summary>
        Game_Story_End,

		/// <summary>
		/// 开始战役<para/>
		/// 参数：关卡ID，是否通关
		/// </summary>
		Game_Campaign_Start,

		/// <summary>
		/// 完成战役<para/>
		/// 参数：关卡ID，原过关星级，新过关星级，是否通关，是否元宝通关
		/// </summary>
		Game_Campaign_Win,

		/// <summary>
		/// 关卡失败<para/>
		/// 参数：关卡ID
		/// </summary>
		Game_Campaign_Fail,

		/// <summary>
		/// 重置精英关<para/>
		/// 参数：关卡ID
		/// </summary>
		Game_Campgign_ResetElite,

        /// <summary>
        /// 进入某一章节
        /// 参数：ChapterData
        /// </summary>
        Game_Chapter_Enter,

		#endregion

		#region Trial

		/// <summary>
		/// 开始试炼<para/>
		/// 参数：关卡ID，是否通关
		/// </summary>
		Game_Trial_Start,

		/// <summary>
		/// 完成试炼<para/>
		/// 参数：关卡ID，原过关星级，新过关星级，是否通关，是否元宝通关
		/// </summary>
		Game_Trial_Win,

		/// <summary>
		/// 试炼失败<para/>
		/// 参数：关卡ID
		/// </summary>
		Game_Trial_Fail,

		#endregion

		#region Arena

		/// <summary>
		/// 竞技开始<para/>
		/// 参数：无
		/// </summary>
		Game_Arena_Start,

		/// <summary>
		/// 竞技结束<para/>
		/// 参数：是否胜利
		/// </summary>
		Game_Arena_End,

		/// <summary>
		/// 购买擂台门票<para/>
		/// 参数：无
		/// </summary>
		Game_Arena_BuyTicket,

		/// <summary>
		/// 重置擂台CD<para/>
		/// 参数：无
		/// </summary>
		Game_Arena_ResetCD,

        /// <summary>
        /// 擂台甘拜下风
        /// </summary>
        Game_Arena_Surrender,

		#endregion

		#region WorldBoss

        /// <summary>
        /// 进入世界Boss
        /// 参数：Boss ID
        /// </summary>
        Game_WorldBoss_Enter,

		/// <summary>
		/// 世界Boss全服伤害值变化<para/>
		/// 参数：WorldBossInfo，新总伤害值
		/// </summary>
		Game_WorldBoss_OnDamageChange,

		/// <summary>
		/// 世界Boss排名变化<para/>
		/// 参数：WorldBossInfo，新排名
		/// </summary>
		Game_WorldBoss_OnRankingChange,

		/// <summary>
		/// 世界Boss排行榜变化<para/>
		/// 参数：WorldBossInfo
		/// </summary>
		Game_WorldBoss_OnLeaderChange,

		/// <summary>
		/// 世界Boss元宝复活<para/>
		/// 参数：WorldBossInfo
		/// </summary>
		Game_WorldBoss_GemRevive,

		/// <summary>
		/// 世界Boss数据更新<para/>
		/// 参数：WorldBossInfo
		/// </summary>
		Game_WorldBoss_Update,

        #endregion

        #region GuildBoss

        /// <summary>
        /// 世界Boss全服伤害值变化<para/>
        /// 参数：WorldBossInfo，新总伤害值
        /// </summary>
        Game_GuildBoss_OnDamageChange,

        /// <summary>
        /// 世界Boss排名变化<para/>
        /// 参数：WorldBossInfo，新排名
        /// </summary>
        Game_GuildBoss_OnRankingChange,

        /// <summary>
        /// 世界Boss排行榜变化<para/>
        /// 参数：WorldBossInfo
        /// </summary>
        Game_GuildBoss_OnLeaderChange,

        /// <summary>
        /// 世界Boss钻石复活<para/>
        /// 参数：WorldBossInfo
        /// </summary>
        Game_GuildBoss_GemRevive,

        /// <summary>
        /// 世界Boss数据更新<para/>
        /// 参数：WorldBossInfo
        /// </summary>
        Game_GuildBoss_Update,

        #endregion

        #region Conquest

        /// <summary>
        /// 远征始<para/>
        /// 参数：无
        /// </summary>
        Game_Conquest_Start,

		/// <summary>
		/// 远征结束<para/>
		/// 参数：是否胜利
		/// </summary>
		Game_Conquest_End,

		/// <summary>
		/// 重置远征CD<para/>
		/// 参数：无
		/// </summary>
		Game_Conquest_ResetCD,

		#endregion

		#region Hero

        /// <summary>
        /// 获得英雄<para/>
        /// 参数：英雄ID
        /// </summary>
        Hero_GetNewHero,

		/// <summary>
		/// 召唤英雄<para/>
		/// 参数：英雄ID
		/// </summary>
		Hero_UnlockHero,

		/// <summary>
		/// 英雄升星<para/>
		/// 参数：英雄ID，原星级，新星级
		/// </summary>
		Hero_UpgradeRank,

		/// <summary>
		/// 英雄进阶<para/>
		/// 参数：英雄ID，原品阶，新品阶
		/// </summary>
		Hero_UpgradeRating,

		/// <summary>
		/// 英雄升级<para/>
		/// 参数：英雄ID，原等级，新等级
		/// </summary>
		Hero_UpgradeLevel,

        /// <summary>
        /// 英雄经脉升级<para/>
        /// 参数：英雄ID，原经脉等级，新经脉等级
        /// </summary>
        Hero_UpgradeMeridianLevel,

		/// <summary>
		/// 英雄技能升级<para/>
		/// 参数：英雄ID，技能序号，原等级，新等级
		/// </summary>
		Hero_UpgradeSkill,

		/// <summary>
		/// 英雄天赋升级<para/>
		/// 参数：英雄ID，天赋序号，原等级，新等级
		/// </summary>
		Hero_UpgradeTalent,

        /// <summary>
        /// 英雄天赋洗练
        /// </summary>
        Hero_ResetTalent,

        /// <summary>
        /// 化混
        /// 参数：原魂晶，新魂晶数量
        /// </summary>
        Hero_GetSophia,

        /// <summary>
        /// 购买天赋锁<para/>
        /// 参数：原数量，现数量
        /// </summary>
        Hero_BuyTalentLock,

		/// <summary>
		/// 英雄穿戴装备<para/>
		/// 参数：英雄ID，装备序号
		/// </summary>
		Hero_WearEquipment,

		/// <summary>
		/// 强化装备<para/>
		/// 参数：英雄ID，装备序号，原等级，新等级
		/// </summary>
		Hero_IntensifyEquipment,

		/// <summary>
		/// 打造装备
		/// </summary>
		Hero_BuildEquipment,

		/// <summary>
		/// 技能点数量变化<para/>
		/// 参数：原数量，新数量
		/// </summary>
		Hero_SkillPointChange,

		/// <summary>
		/// 购买技能点<para/>
		/// 参数：无
		/// </summary>
		Hero_BuySkillPoint,

        /// <summary>
        /// 英雄天赋可升级<para/>
        /// 参数：无
        /// </summary>
        Hero_CanUpgradeTalent,

        /// <summary>
        /// 升级羁绊技能
        /// 参数：原技能等级，当前技能等级
        /// </summary>
        Hero_UpgradeComboSkill,

		/// <summary>
		/// 传记解锁
		/// </summary>
		Hero_UnLockHistory,

        /// <summary>
        /// 提升真言等级
        /// 参数：英雄ID， 真言ID，原等级，当前等级
        /// </summary>
        Hero_UpgradeMantra,

        /// <summary>
        /// 提升真言下属Buff等级
        /// 参数：英雄ID， BuffID，原等级，当前等级
        /// </summary>
        Hero_UpgradeMantraBuff,

        #endregion

        #region Item

        /// <summary>
        /// 物品数量变化<para/>
        /// 参数：物品ID，原数量，新数量
        /// </summary>
        Item_AmountChange,

		/// <summary>
		/// 使用通关券<para/>
		/// 参数：物品ID，使用的数量，通关的关卡ID
		/// </summary>
		Item_SweepTicket_Use,

		/// <summary>
		/// 使用经验药水<para/>
		/// 参数：物品ID，使用的数量，英雄ID
		/// </summary>
		Item_ExpPotion_Use,

		/// <summary>
		/// 使用体力药水<para/>
		/// 参数：物品ID，使用的数量
		/// </summary>
		Item_StaPotion_Use,

		/// <summary>
		/// 使用月卡体验券<para/>
		/// 参数：物品ID，使用的数量
		/// </summary>
		Item_MonthCardTicket_Use,

		/// <summary>
		/// 出售物品<para/>
		/// 参数：物品ID，出售数量，获得金币
		/// </summary>
		Item_Sell,

		/// <summary>
		/// 合成物品<para/>
		/// 参数：物品ID
		/// </summary>
		Item_Fuse,

        /// <summary>
		/// 交纳物品
		/// 参数：物品ID
		/// </summary>
		Item_DevoteItem,

        /// <summary>
		/// 捐钱
		/// 参数：金币
		/// </summary>
		Item_DevoteGold,

        #endregion

        #region Shop

        /// <summary>
        /// 购买商品<para/>
        /// 参数：商店ID，商品ID
        /// </summary>
        Shop_BuyItem,

		/// <summary>
		/// 商店手动刷新<para/>
		/// 参数：商店ID
		/// </summary>
		Shop_RefreshItem,

		#endregion

		#region Altar

		/// <summary>
		/// 祭坛免费召唤<para/>
		/// 参数：祭坛ID，次数
		/// </summary>
		Altar_FreeCall,

		/// <summary>
		/// 祭坛付费召唤<para/>
		/// 参数：祭坛ID，次数
		/// </summary>
		Altar_Call_1,

		/// <summary>
		/// 祭坛付费十连召唤<para/>
		/// 参数：祭坛ID，次数
		/// </summary>
		Altar_Call_10,

		#endregion

		#region Task

		/// <summary>
		/// 领取任务奖励<para/>
		/// 参数：任务ID
		/// </summary>
		Task_GetTaskReward,

		/// <summary>
		/// 领取每日任务奖励<para/>
		/// 参数：任务ID
		/// </summary>
		Task_GetDailyTaskReward,

		/// <summary>
		/// 领取活动奖励<para/>
		/// 参数：活动ID，任务ID,活动类型
		/// </summary>
		Activity_GetTaskReward,

        #endregion

        #region ImmortalTask

        /// <summary>
		/// 开始仙魔榜任务<para/>
		/// 参数：任务ID
		/// </summary>
		ImmortalTask_Start,

        /// <summary>
		/// 领取仙魔榜任务奖励<para/>
		/// 参数：任务ID
		/// </summary>
		ImmortalTask_GetTaskReward,

        /// <summary>
		/// 立即完成仙魔榜任务<para/>
		/// 参数：任务ID
		/// </summary>
		ImmortalTask_FinishNow,

        /// <summary>
		/// 放弃仙魔榜任务<para/>
		/// 参数：任务ID
		/// </summary>
		ImmortalTask_GiveUp,

        #endregion

	    #region RingTask
        /// <summary>
        /// 开启跑环任务
        /// </summary>
        RingTask_Start,

        /// <summary>
        /// 立即完成任务
        /// </summary>
        RingTask_FinishNow,

        /// <summary>
        /// 放弃任务
        /// </summary>
        RingTask_GiveUp,

        /// <summary>
        /// 获得奖励
        /// </summary>
        RingTask_GetTaskReward,

	    #endregion

        #region 

        /// <summary>
        /// 初始化帮会信息<para/>
        /// 参数：玩家名称
        /// </summary>
        Guild,
		/// <summary>
		/// 任务完成
		/// </summary>
		Task_Finish,
		#endregion

		#region Guild

		/// <summary>
		/// 初始化帮会信息<para/>
		/// 参数：玩家名称
		/// </summary>
		Guild_OnInitInfo,

		/// <summary>
		/// 有新入会请求<para/>
		/// 参数：玩家名称
		/// </summary>
		Guild_OnRequest,

		/// <summary>
		/// 被同意加入帮会<para/>
		/// 参数：帮会ID，帮会名称，操作者名称
		/// </summary>
		Guild_OnAccept,

		/// <summary>
		/// 被拒绝加入帮会<para/>
		/// 参数：帮会ID，帮会名称，操作者名称
		/// </summary>
		Guild_OnRefuse,

		/// <summary>
		/// 有玩家被同意加入帮会<para/>
		/// 参数：玩家ID，操作者名称
		/// </summary>
		Guild_OnMemberAccept,

		/// <summary>
		/// 有玩家主动进入帮会<para/>
		/// 参数：玩家ID
		/// </summary>
		Guild_OnMemberEnter,

		/// <summary>
		/// 有玩家主动离开帮会<para/>
		/// 参数：玩家ID，玩家名称（可能不存在本地缓存）
		/// </summary>
		Guild_OnMemberLeave,

		/// <summary>
		/// 被踢出帮会<para/>
		/// 参数：帮会ID，帮会名称，操作者名称
		/// </summary>
		Guild_OnKickout,

		/// <summary>
		/// 有玩家被踢出帮会<para/>
		/// 参数：玩家ID，玩家名称，操作者名称（可能不存在本地缓存）
		/// </summary>
		Guild_OnMemberKickout,

		/// <summary>
		/// 帮主转让<para/>
		/// 参数：玩家ID，玩家名称，操作者ID，操作者名称（可能不存在本地缓存）
		/// </summary>
		Guild_OnMasterChange,

		/// <summary>
		/// 职权变更<para/>
		/// 参数：玩家ID，玩家名称，旧职权，新职权，操作者名称（可能不存在本地缓存）
		/// </summary>
		Guild_OnDutyChange,

        /// <summary>
        /// 帮会祈福
        /// 参数：（金币祈福==1，元宝祈福==2）
        /// </summary>
        Guild_Pray,

		/// <summary>
		/// 帮会捐赠
		/// </summary>
		Guild_GetDailyReward,
		
		/// <summary>
		/// 帮会帮主请求
		/// </summary>
		Guild_OnApplyMaster,

        /// <summary>
		/// 跨服战开始<para/>
		/// 参数：无
		/// </summary>
		Game_AcrossFight_Start,

        /// <summary>
        /// 跨服战结束<para/>
        /// 参数：是否胜利
        /// </summary>
        Game_AcrossFight_End,

        /// <summary>
        /// 跨服战甘拜下风
        /// </summary>
        Game_AcrossFight_Surrender,

        #endregion

        #region Chat

        /// <summary>
        /// 有公共聊天消息<para/>
        /// 参数：ChatMsg
        /// </summary>
        Chat_OnPublicMsg,

		/// <summary>
		/// 有帮会聊天消息<para/>
		/// 参数：ChatMsg
		/// </summary>
		Chat_OnGuildMsg,

		/// <summary>
		/// 有私人聊天消息<para/>
		/// 参数：PrivateChatSession, ChatMsg
		/// </summary>
		Chat_OnPrivateMsg,

		/// <summary>
		/// 接收到新广播<para/>
		/// 参数：广播消息
		/// </summary>
		BroadCast_OnMsg,
        /// <summary>
        /// 幻世伏魔聊天
        /// </summary>
        Chat_OnWorldBossMsg,

        /// <summary>
        /// 发送消息
        /// 参数：类型，消息内容
        /// </summary>
        Chat_SendMsg,

		#endregion

		#region Friend

		/// <summary>
		/// 有新好友请求<para/>
		/// 参数：FriendRequest
		/// </summary>
		Friend_OnRequest,

		/// <summary>
		/// 加新好友<para/>
		/// 参数：FriendInfo
		/// </summary>
		Friend_OnNewFriend,

		/// <summary>
		/// 删除好友<para/>
		/// 参数：FriendInfo
		/// </summary>
		Friend_OnDelete,

		/// <summary>
		/// 有好友赠送体力<para/>
		/// 参数：FriendInfo, 赠送的体力
		/// </summary>
		Friend_OnReceiveSta,

        /// <summary>
        /// 赠送好友体力
        /// </summary>
        Friend_OnSendStamina,

        /// <summary>
		/// 有好友赠送礼物<para/>
		/// 参数：FriendInfo, FriendGiftInfo
		/// </summary>
		Friend_OnReceiveGift,

        /// <summary>
        /// 有好友领取送礼物<para/>
        /// 参数：FriendInfo
        /// </summary>
        Friend_OnGiftGet,

        /// <summary>
        /// 赠送物品
        /// 参数：FriendInfo, 物品ID, 数量
        /// </summary>
        Friend_SendGift,

        #endregion

        #region Tutorial
        /// <summary>
        /// 显示CG中对话
        /// 参数：对话ID
        /// </summary>
        CG_ShowStoryDialog,

		/// <summary>
		/// CG结束
		/// </summary>
		CG_END,

        /// <summary>
        /// 跳过初始教程关卡
        /// </summary>
        TutorialLevel_END,

		/// <summary>
		/// 跳过镜头展示
		/// </summary>
		CG_SkipCameraShow,
		/// <summary>
		/// 跳过新手引导
		/// </summary>
		CG_SkipTutorial,
        #endregion

        #region Achieve

        /// <summary>
        /// 膜拜英雄
        /// </summary>
        Achieve_Worship,
        /// <summary>
        /// 更换成就头衔
        /// </summary>
        Achieve_ChangeName,

        /// <summary>
        /// 更换成就边框
        /// </summary>
        Achieve_ChangeFrame,

        #endregion

        #region MoneyTree
        MoneyTree_DropCoin,
        MoneyTree_PrayStatus,
        MoneyTree_GetMoney,
        MoneyTree_Shake,
        MoneyTree_Pray,
        MoneyTree_StopDropCoin,
        MoneyTree_Reset,
		MoneyTree_LevelUp,
        #endregion  

		#region Glory
		/// <summary>
		/// 封神台开始<para/>
		/// 参数：无
		/// </summary>
		Glory_GloryStart,
		/// <summary>
		/// 封神台结束<para/>
		/// 参数：是否胜利
		/// </summary>
		Glory_GloryEnd,

		/// <summary>
		/// 购买封神台门票<para/>
		/// 参数：无
		/// </summary>
		Glory_BuyTicket,
        #endregion

        #region Mine

        /// <summary>
        /// 资源战开始<para/>
        /// 参数：无
        /// </summary>
        Game_Mine_Start,

        /// <summary>
        /// 资源战结束<para/>
        /// 参数：是否胜利
        /// </summary>
        Game_Mine_End,

        #endregion

        #region MainTask
        /// <summary>
        /// 完成主线任务
        /// </summary>
        Task_CompleteMainTask,

        /// <summary>
        /// 主线对话任务
        /// </summary>
        Task_Talk,

        /// <summary>
        /// 主线战役
        /// 参数：战役id， 战斗星级
        /// </summary>
        Task_Campaign,

        /// <summary>
        /// 主线任务穿装备
        /// </summary>
        Task_WearEquip,

        /// <summary>
        /// 主线任务英雄召唤
        /// </summary>
        Task_AltarCall,

        /// <summary>
        /// 主线任务打造装备
        /// </summary>
        Task_BuildEquip,

        /// <summary>
        /// 分解装备
        /// </summary>
        Task_BreakupEquip,

        /// <summary>
        /// 技能升级
        /// </summary>
        Task_SkillLevelup,

        /// <summary>
        /// 挑战练武台
        /// </summary>
        Task_ChallengeArena,

        /// <summary>
        /// 设置练武台队伍
        /// </summary>
        Task_SetArenaTeam,

        /// <summary>
        /// 使用摇钱树
        /// </summary>
        Task_UseMoneyTree,

        /// <summary>
        /// 元神升级
        /// </summary>
        Task_SpiritUp,

        /// <summary>
        /// 挑战玄霄仙境
        /// </summary>
        Task_ChallengeSupply,

        /// <summary>
        /// 挑战幻境附魔
        /// </summary>
        Task_ChallengeEquipment,

        /// <summary>
        /// 装备强化
        /// </summary>
        Task_EquipIntensify,

        /// <summary>
        /// 经脉升级
        /// </summary>
        Task_MeridianUp,

        /// <summary>
        /// 经脉进阶
        /// </summary>
        Task_MeridianRank,

        /// <summary>
        /// 加入或者创建帮会
        /// </summary>
        Task_JoinCreateGuild,

        /// <summary>
        /// 挑战登仙台
        /// </summary>
        Task_ChallengeConquest,

        /// <summary>
        /// 挑战秘境猎魔
        /// </summary>
        Task_ChallengeWorldBoss,

        /// <summary>
        /// 挑战封神台
        /// </summary>
        Task_ChallengeGlory,

        /// <summary>
        /// 战斗失败
        /// </summary>
        Task_BattleFail,

		/// <summary>
		/// 解锁英雄
		/// </summary>
		Task_UnLockHero,

        #endregion

        #region Buff

        /// <summary>
        /// Buff有更新
        /// </summary>
        Buff_Update,

        #endregion

        #region Artifact

        /// <summary>
        /// 添加神器<para/>
        /// 参数：神器ID，神器序号
        /// </summary>
        Artifact_Add,

        /// <summary>
        /// 强化神器<para/>
        /// 参数：神器ID，神器序号，原等级，新等级
        /// </summary>
        Artifact_Intensify,

        /// <summary>
        /// 突破神器<para/>
        /// 参数：神器ID，神器序号，是否成功，当前等级
        /// </summary>
        Artifact_Breach,

        /// <summary>
        /// 洗练神器<para/>
        /// 参数：神器ID，神器序号，洗练技能个数
        /// </summary>
        Artifact_ResetSkill,

        /// <summary>
        /// 设置主神器<para/>
        /// 参数：当前神器ID
        /// </summary>
        Artifact_SetMain,

        /// <summary>
        /// 回收神器技能<para/>
        /// 参数：当前神器ID
        /// </summary>
        Artifact_ExchangeSkill,

        /// <summary>
        /// 同步主神器信息
        /// 参数：神器ID，神器序号
        /// </summary>
        Artifact_SyncInfo,

        #endregion

        #region RingTask

        /// <summary>
        /// 刷新信息
        /// </summary>
        RingTask_RefreshInfo,

        /// <summary>
        /// 完成环任务
        /// </summary>
        RingTask_CompleteTask,

        /// <summary>
        /// 环对话任务
        /// </summary>
        RingTask_Talk,

        /// <summary>
        /// 环任务战役
        /// 参数：战役id， 战斗星级
        /// </summary>
        RingTask_Campaign,

		#endregion
    }
}