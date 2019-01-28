public enum eTABLE_LIST
{
	CHAR_INFO,
	CHAR_LEVEL_ADD,
	CHAR_LEVEL_DIV,
	CHAR_LEVEL_MUL,
	CHAR_LEVEL_SUB,
	INFINITY_RANDOM,
	ITEM_ID,
	ITEM_TABLE,
	LANGUAGE,
	LANGUAGE_TABLE,
	REWARD_TABLE,
	SALE_TYPE,
	STAGE_LEVEL_1,
	STAGE_LEVEL_10,
	STAGE_LEVEL_11,
	STAGE_LEVEL_12,
	STAGE_LEVEL_13,
	STAGE_LEVEL_14,
	STAGE_LEVEL_15,
	STAGE_LEVEL_16,
	STAGE_LEVEL_17,
	STAGE_LEVEL_18,
	STAGE_LEVEL_19,
	STAGE_LEVEL_2,
	STAGE_LEVEL_20,
	STAGE_LEVEL_21,
	STAGE_LEVEL_22,
	STAGE_LEVEL_23,
	STAGE_LEVEL_24,
	STAGE_LEVEL_25,
	STAGE_LEVEL_26,
	STAGE_LEVEL_27,
	STAGE_LEVEL_28,
	STAGE_LEVEL_29,
	STAGE_LEVEL_3,
	STAGE_LEVEL_30,
	STAGE_LEVEL_31,
	STAGE_LEVEL_32,
	STAGE_LEVEL_33,
	STAGE_LEVEL_34,
	STAGE_LEVEL_35,
	STAGE_LEVEL_36,
	STAGE_LEVEL_37,
	STAGE_LEVEL_38,
	STAGE_LEVEL_39,
	STAGE_LEVEL_4,
	STAGE_LEVEL_40,
	STAGE_LEVEL_41,
	STAGE_LEVEL_42,
	STAGE_LEVEL_43,
	STAGE_LEVEL_44,
	STAGE_LEVEL_45,
	STAGE_LEVEL_46,
	STAGE_LEVEL_47,
	STAGE_LEVEL_48,
	STAGE_LEVEL_49,
	STAGE_LEVEL_5,
	STAGE_LEVEL_50,
	STAGE_LEVEL_6,
	STAGE_LEVEL_7,
	STAGE_LEVEL_8,
	STAGE_LEVEL_9,
	TITLE_MARK,
	END
}

public enum eKEY_TABLEDB
{
	f_ACTIVATE_DURARION,
	f_COOLDOWN_DURATION,
	f_ITEM_PRICE_US,
	f_SPEED_VALUE,
	i_AMOUNT,
	i_EXAM_FTRANDOM_MAX,
	i_EXAM_FTRANDOM_MIN,
	i_EXAM_MANUAL_1,
	i_EXAM_MANUAL_2,
	i_EXAM_MANUAL_3,
	i_EXAM_MANUAL_4,
	i_EXAM_MANUAL_5,
	i_EXAM_NUM,
	i_ID,
	i_ITEM_ID,
	i_ITEM_LIMIT,
	i_ITEM_PRICE_TYPE,
	i_ITEM_SET_VALUE,
	i_LIFE_VALUE,
	i_LINE_ID,
	i_LV,
	i_NEED_ITEM,
	i_PERCENTAGE,
	i_REWARD_1,
	i_REWARD_1_VALUE,
	i_REWARD_2,
	i_REWARD_2_VALUE,
	i_REWARD_3,
	i_REWARD_3_VALUE,
	i_REWARD_AMOUNT,
	i_REWARD_ID,
	i_SKILL_VALUE,
	i_SPEED_VALUE,
	i_STAGE,
	i_STRING_ID,
	i_TYPE,
	i_TYPE_VALUE,
	i_UNLOCK_ID,
	i_UNLOCK_STAGE_INFO,
	s_APPEARTIME,
	s_CHAR_NAME_KR,
	s_CHAR_NAME_US,
	s_CHAR_STORY_KR,
	s_CHAR_STORY_US,
	s_CONDITION,
	s_ENUM_VALUE,
	s_GRADE_NAME_KR,
	s_GRADE_NAME_US,
	s_ITEM_INFO_KR,
	s_ITEM_INFO_US,
	s_ITEM_NAME_KR,
	s_ITEM_NAME_US,
	s_LANGUAGE_AT,
	s_LANGUAGE_ENG,
	s_LANGUAGE_KOR,
	s_RESOURCE,
	s_SKILL_KR,
	s_SKILL_US,
	s_TITLE_NAME_KR,
	s_TITLE_NAME_US,
	END
}

public enum CHAR_INFO
{
	i_ID,
	s_CHAR_NAME_KR,
	s_CHAR_NAME_US,
	s_CHAR_STORY_KR,
	s_CHAR_STORY_US,
	s_SKILL_KR,
	s_SKILL_US,
	i_UNLOCK_STAGE_INFO,
	END
}

public enum CHAR_LEVEL_ADD
{
	i_LV,
	i_NEED_ITEM,
	i_AMOUNT,
	i_TYPE,
	i_SKILL_VALUE,
	i_TYPE_VALUE,
	i_LIFE_VALUE,
	END
}

public enum CHAR_LEVEL_DIV
{
	i_LV,
	i_NEED_ITEM,
	i_AMOUNT,
	i_TYPE,
	i_SKILL_VALUE,
	i_TYPE_VALUE,
	i_LIFE_VALUE,
	END
}

public enum CHAR_LEVEL_MUL
{
	i_LV,
	i_NEED_ITEM,
	i_AMOUNT,
	i_TYPE,
	i_SKILL_VALUE,
	i_TYPE_VALUE,
	i_LIFE_VALUE,
	END
}

public enum CHAR_LEVEL_SUB
{
	i_LV,
	i_NEED_ITEM,
	i_AMOUNT,
	i_TYPE,
	i_SKILL_VALUE,
	i_TYPE_VALUE,
	i_LIFE_VALUE,
	END
}

public enum INFINITY_RANDOM
{
	i_SPEED_VALUE,
	i_PERCENTAGE,
	END
}

public enum ITEM_ID
{
	i_ID,
	s_ITEM_NAME_KR,
	s_ITEM_NAME_US,
	f_ACTIVATE_DURARION,
	f_COOLDOWN_DURATION,
	END
}

public enum ITEM_TABLE
{
	i_ID,
	s_ITEM_NAME_KR,
	s_ITEM_NAME_US,
	s_ITEM_INFO_KR,
	s_ITEM_INFO_US,
	i_ITEM_ID,
	i_ITEM_SET_VALUE,
	i_ITEM_LIMIT,
	i_ITEM_PRICE_TYPE,
	f_ITEM_PRICE_US,
	END
}

public enum LANGUAGE
{
	i_STRING_ID,
	s_LANGUAGE_KOR,
	s_LANGUAGE_ENG,
	END
}

public enum LANGUAGE_TABLE
{
	i_STRING_ID,
	s_LANGUAGE_AT,
	s_ENUM_VALUE,
	s_LANGUAGE_KOR,
	s_LANGUAGE_ENG,
	END
}

public enum REWARD_TABLE
{
	i_STAGE,
	i_REWARD_1,
	i_REWARD_1_VALUE,
	i_REWARD_2,
	i_REWARD_2_VALUE,
	i_REWARD_3,
	i_REWARD_3_VALUE,
	i_UNLOCK_ID,
	END
}

public enum SALE_TYPE
{
	i_TYPE,
	s_ITEM_NAME_KR,
	s_ITEM_NAME_US,
	END
}

public enum STAGE_LEVEL_1
{
	i_LINE_ID,
	i_EXAM_NUM,
	i_EXAM_MANUAL_1,
	i_EXAM_MANUAL_2,
	i_EXAM_MANUAL_3,
	i_EXAM_MANUAL_4,
	i_EXAM_MANUAL_5,
	i_EXAM_FTRANDOM_MIN,
	i_EXAM_FTRANDOM_MAX,
	s_APPEARTIME,
	f_SPEED_VALUE,
	END
}

public enum STAGE_LEVEL_10
{
	i_LINE_ID,
	i_EXAM_NUM,
	i_EXAM_MANUAL_1,
	i_EXAM_MANUAL_2,
	i_EXAM_MANUAL_3,
	i_EXAM_MANUAL_4,
	i_EXAM_MANUAL_5,
	i_EXAM_FTRANDOM_MIN,
	i_EXAM_FTRANDOM_MAX,
	s_APPEARTIME,
	f_SPEED_VALUE,
	END
}

public enum STAGE_LEVEL_11
{
	i_LINE_ID,
	i_EXAM_NUM,
	i_EXAM_MANUAL_1,
	i_EXAM_MANUAL_2,
	i_EXAM_MANUAL_3,
	i_EXAM_MANUAL_4,
	i_EXAM_MANUAL_5,
	i_EXAM_FTRANDOM_MIN,
	i_EXAM_FTRANDOM_MAX,
	s_APPEARTIME,
	f_SPEED_VALUE,
	END
}

public enum STAGE_LEVEL_12
{
	i_LINE_ID,
	i_EXAM_NUM,
	i_EXAM_MANUAL_1,
	i_EXAM_MANUAL_2,
	i_EXAM_MANUAL_3,
	i_EXAM_MANUAL_4,
	i_EXAM_MANUAL_5,
	i_EXAM_FTRANDOM_MIN,
	i_EXAM_FTRANDOM_MAX,
	s_APPEARTIME,
	f_SPEED_VALUE,
	END
}

public enum STAGE_LEVEL_13
{
	i_LINE_ID,
	i_EXAM_NUM,
	i_EXAM_MANUAL_1,
	i_EXAM_MANUAL_2,
	i_EXAM_MANUAL_3,
	i_EXAM_MANUAL_4,
	i_EXAM_MANUAL_5,
	i_EXAM_FTRANDOM_MIN,
	i_EXAM_FTRANDOM_MAX,
	s_APPEARTIME,
	f_SPEED_VALUE,
	END
}

public enum STAGE_LEVEL_14
{
	i_LINE_ID,
	i_EXAM_NUM,
	i_EXAM_MANUAL_1,
	i_EXAM_MANUAL_2,
	i_EXAM_MANUAL_3,
	i_EXAM_MANUAL_4,
	i_EXAM_MANUAL_5,
	i_EXAM_FTRANDOM_MIN,
	i_EXAM_FTRANDOM_MAX,
	s_APPEARTIME,
	f_SPEED_VALUE,
	END
}

public enum STAGE_LEVEL_15
{
	i_LINE_ID,
	i_EXAM_NUM,
	i_EXAM_MANUAL_1,
	i_EXAM_MANUAL_2,
	i_EXAM_MANUAL_3,
	i_EXAM_MANUAL_4,
	i_EXAM_MANUAL_5,
	i_EXAM_FTRANDOM_MIN,
	i_EXAM_FTRANDOM_MAX,
	s_APPEARTIME,
	f_SPEED_VALUE,
	END
}

public enum STAGE_LEVEL_16
{
	i_LINE_ID,
	i_EXAM_NUM,
	i_EXAM_MANUAL_1,
	i_EXAM_MANUAL_2,
	i_EXAM_MANUAL_3,
	i_EXAM_MANUAL_4,
	i_EXAM_MANUAL_5,
	i_EXAM_FTRANDOM_MIN,
	i_EXAM_FTRANDOM_MAX,
	s_APPEARTIME,
	f_SPEED_VALUE,
	END
}

public enum STAGE_LEVEL_17
{
	i_LINE_ID,
	i_EXAM_NUM,
	i_EXAM_MANUAL_1,
	i_EXAM_MANUAL_2,
	i_EXAM_MANUAL_3,
	i_EXAM_MANUAL_4,
	i_EXAM_MANUAL_5,
	i_EXAM_FTRANDOM_MIN,
	i_EXAM_FTRANDOM_MAX,
	s_APPEARTIME,
	f_SPEED_VALUE,
	END
}

public enum STAGE_LEVEL_18
{
	i_LINE_ID,
	i_EXAM_NUM,
	i_EXAM_MANUAL_1,
	i_EXAM_MANUAL_2,
	i_EXAM_MANUAL_3,
	i_EXAM_MANUAL_4,
	i_EXAM_MANUAL_5,
	i_EXAM_FTRANDOM_MIN,
	i_EXAM_FTRANDOM_MAX,
	s_APPEARTIME,
	f_SPEED_VALUE,
	END
}

public enum STAGE_LEVEL_19
{
	i_LINE_ID,
	i_EXAM_NUM,
	i_EXAM_MANUAL_1,
	i_EXAM_MANUAL_2,
	i_EXAM_MANUAL_3,
	i_EXAM_MANUAL_4,
	i_EXAM_MANUAL_5,
	i_EXAM_FTRANDOM_MIN,
	i_EXAM_FTRANDOM_MAX,
	s_APPEARTIME,
	f_SPEED_VALUE,
	END
}

public enum STAGE_LEVEL_2
{
	i_LINE_ID,
	i_EXAM_NUM,
	i_EXAM_MANUAL_1,
	i_EXAM_MANUAL_2,
	i_EXAM_MANUAL_3,
	i_EXAM_MANUAL_4,
	i_EXAM_MANUAL_5,
	i_EXAM_FTRANDOM_MIN,
	i_EXAM_FTRANDOM_MAX,
	s_APPEARTIME,
	f_SPEED_VALUE,
	END
}

public enum STAGE_LEVEL_20
{
	i_LINE_ID,
	i_EXAM_NUM,
	i_EXAM_MANUAL_1,
	i_EXAM_MANUAL_2,
	i_EXAM_MANUAL_3,
	i_EXAM_MANUAL_4,
	i_EXAM_MANUAL_5,
	i_EXAM_FTRANDOM_MIN,
	i_EXAM_FTRANDOM_MAX,
	s_APPEARTIME,
	f_SPEED_VALUE,
	END
}

public enum STAGE_LEVEL_21
{
	i_LINE_ID,
	i_EXAM_NUM,
	i_EXAM_MANUAL_1,
	i_EXAM_MANUAL_2,
	i_EXAM_MANUAL_3,
	i_EXAM_MANUAL_4,
	i_EXAM_MANUAL_5,
	i_EXAM_FTRANDOM_MIN,
	i_EXAM_FTRANDOM_MAX,
	s_APPEARTIME,
	f_SPEED_VALUE,
	END
}

public enum STAGE_LEVEL_22
{
	i_LINE_ID,
	i_EXAM_NUM,
	i_EXAM_MANUAL_1,
	i_EXAM_MANUAL_2,
	i_EXAM_MANUAL_3,
	i_EXAM_MANUAL_4,
	i_EXAM_MANUAL_5,
	i_EXAM_FTRANDOM_MIN,
	i_EXAM_FTRANDOM_MAX,
	s_APPEARTIME,
	f_SPEED_VALUE,
	END
}

public enum STAGE_LEVEL_23
{
	i_LINE_ID,
	i_EXAM_NUM,
	i_EXAM_MANUAL_1,
	i_EXAM_MANUAL_2,
	i_EXAM_MANUAL_3,
	i_EXAM_MANUAL_4,
	i_EXAM_MANUAL_5,
	i_EXAM_FTRANDOM_MIN,
	i_EXAM_FTRANDOM_MAX,
	s_APPEARTIME,
	f_SPEED_VALUE,
	END
}

public enum STAGE_LEVEL_24
{
	i_LINE_ID,
	i_EXAM_NUM,
	i_EXAM_MANUAL_1,
	i_EXAM_MANUAL_2,
	i_EXAM_MANUAL_3,
	i_EXAM_MANUAL_4,
	i_EXAM_MANUAL_5,
	i_EXAM_FTRANDOM_MIN,
	i_EXAM_FTRANDOM_MAX,
	s_APPEARTIME,
	f_SPEED_VALUE,
	END
}

public enum STAGE_LEVEL_25
{
	i_LINE_ID,
	i_EXAM_NUM,
	i_EXAM_MANUAL_1,
	i_EXAM_MANUAL_2,
	i_EXAM_MANUAL_3,
	i_EXAM_MANUAL_4,
	i_EXAM_MANUAL_5,
	i_EXAM_FTRANDOM_MIN,
	i_EXAM_FTRANDOM_MAX,
	s_APPEARTIME,
	f_SPEED_VALUE,
	END
}

public enum STAGE_LEVEL_26
{
	i_LINE_ID,
	i_EXAM_NUM,
	i_EXAM_MANUAL_1,
	i_EXAM_MANUAL_2,
	i_EXAM_MANUAL_3,
	i_EXAM_MANUAL_4,
	i_EXAM_MANUAL_5,
	i_EXAM_FTRANDOM_MIN,
	i_EXAM_FTRANDOM_MAX,
	s_APPEARTIME,
	f_SPEED_VALUE,
	END
}

public enum STAGE_LEVEL_27
{
	i_LINE_ID,
	i_EXAM_NUM,
	i_EXAM_MANUAL_1,
	i_EXAM_MANUAL_2,
	i_EXAM_MANUAL_3,
	i_EXAM_MANUAL_4,
	i_EXAM_MANUAL_5,
	i_EXAM_FTRANDOM_MIN,
	i_EXAM_FTRANDOM_MAX,
	s_APPEARTIME,
	f_SPEED_VALUE,
	END
}

public enum STAGE_LEVEL_28
{
	i_LINE_ID,
	i_EXAM_NUM,
	i_EXAM_MANUAL_1,
	i_EXAM_MANUAL_2,
	i_EXAM_MANUAL_3,
	i_EXAM_MANUAL_4,
	i_EXAM_MANUAL_5,
	i_EXAM_FTRANDOM_MIN,
	i_EXAM_FTRANDOM_MAX,
	s_APPEARTIME,
	f_SPEED_VALUE,
	END
}

public enum STAGE_LEVEL_29
{
	i_LINE_ID,
	i_EXAM_NUM,
	i_EXAM_MANUAL_1,
	i_EXAM_MANUAL_2,
	i_EXAM_MANUAL_3,
	i_EXAM_MANUAL_4,
	i_EXAM_MANUAL_5,
	i_EXAM_FTRANDOM_MIN,
	i_EXAM_FTRANDOM_MAX,
	s_APPEARTIME,
	f_SPEED_VALUE,
	END
}

public enum STAGE_LEVEL_3
{
	i_LINE_ID,
	i_EXAM_NUM,
	i_EXAM_MANUAL_1,
	i_EXAM_MANUAL_2,
	i_EXAM_MANUAL_3,
	i_EXAM_MANUAL_4,
	i_EXAM_MANUAL_5,
	i_EXAM_FTRANDOM_MIN,
	i_EXAM_FTRANDOM_MAX,
	s_APPEARTIME,
	f_SPEED_VALUE,
	END
}

public enum STAGE_LEVEL_30
{
	i_LINE_ID,
	i_EXAM_NUM,
	i_EXAM_MANUAL_1,
	i_EXAM_MANUAL_2,
	i_EXAM_MANUAL_3,
	i_EXAM_MANUAL_4,
	i_EXAM_MANUAL_5,
	i_EXAM_FTRANDOM_MIN,
	i_EXAM_FTRANDOM_MAX,
	s_APPEARTIME,
	f_SPEED_VALUE,
	END
}

public enum STAGE_LEVEL_31
{
	i_LINE_ID,
	i_EXAM_NUM,
	i_EXAM_MANUAL_1,
	i_EXAM_MANUAL_2,
	i_EXAM_MANUAL_3,
	i_EXAM_MANUAL_4,
	i_EXAM_MANUAL_5,
	i_EXAM_FTRANDOM_MIN,
	i_EXAM_FTRANDOM_MAX,
	s_APPEARTIME,
	f_SPEED_VALUE,
	END
}

public enum STAGE_LEVEL_32
{
	i_LINE_ID,
	i_EXAM_NUM,
	i_EXAM_MANUAL_1,
	i_EXAM_MANUAL_2,
	i_EXAM_MANUAL_3,
	i_EXAM_MANUAL_4,
	i_EXAM_MANUAL_5,
	i_EXAM_FTRANDOM_MIN,
	i_EXAM_FTRANDOM_MAX,
	s_APPEARTIME,
	f_SPEED_VALUE,
	END
}

public enum STAGE_LEVEL_33
{
	i_LINE_ID,
	i_EXAM_NUM,
	i_EXAM_MANUAL_1,
	i_EXAM_MANUAL_2,
	i_EXAM_MANUAL_3,
	i_EXAM_MANUAL_4,
	i_EXAM_MANUAL_5,
	i_EXAM_FTRANDOM_MIN,
	i_EXAM_FTRANDOM_MAX,
	s_APPEARTIME,
	f_SPEED_VALUE,
	END
}

public enum STAGE_LEVEL_34
{
	i_LINE_ID,
	i_EXAM_NUM,
	i_EXAM_MANUAL_1,
	i_EXAM_MANUAL_2,
	i_EXAM_MANUAL_3,
	i_EXAM_MANUAL_4,
	i_EXAM_MANUAL_5,
	i_EXAM_FTRANDOM_MIN,
	i_EXAM_FTRANDOM_MAX,
	s_APPEARTIME,
	f_SPEED_VALUE,
	END
}

public enum STAGE_LEVEL_35
{
	i_LINE_ID,
	i_EXAM_NUM,
	i_EXAM_MANUAL_1,
	i_EXAM_MANUAL_2,
	i_EXAM_MANUAL_3,
	i_EXAM_MANUAL_4,
	i_EXAM_MANUAL_5,
	i_EXAM_FTRANDOM_MIN,
	i_EXAM_FTRANDOM_MAX,
	s_APPEARTIME,
	f_SPEED_VALUE,
	END
}

public enum STAGE_LEVEL_36
{
	i_LINE_ID,
	i_EXAM_NUM,
	i_EXAM_MANUAL_1,
	i_EXAM_MANUAL_2,
	i_EXAM_MANUAL_3,
	i_EXAM_MANUAL_4,
	i_EXAM_MANUAL_5,
	i_EXAM_FTRANDOM_MIN,
	i_EXAM_FTRANDOM_MAX,
	s_APPEARTIME,
	f_SPEED_VALUE,
	END
}

public enum STAGE_LEVEL_37
{
	i_LINE_ID,
	i_EXAM_NUM,
	i_EXAM_MANUAL_1,
	i_EXAM_MANUAL_2,
	i_EXAM_MANUAL_3,
	i_EXAM_MANUAL_4,
	i_EXAM_MANUAL_5,
	i_EXAM_FTRANDOM_MIN,
	i_EXAM_FTRANDOM_MAX,
	s_APPEARTIME,
	f_SPEED_VALUE,
	END
}

public enum STAGE_LEVEL_38
{
	i_LINE_ID,
	i_EXAM_NUM,
	i_EXAM_MANUAL_1,
	i_EXAM_MANUAL_2,
	i_EXAM_MANUAL_3,
	i_EXAM_MANUAL_4,
	i_EXAM_MANUAL_5,
	i_EXAM_FTRANDOM_MIN,
	i_EXAM_FTRANDOM_MAX,
	s_APPEARTIME,
	f_SPEED_VALUE,
	END
}

public enum STAGE_LEVEL_39
{
	i_LINE_ID,
	i_EXAM_NUM,
	i_EXAM_MANUAL_1,
	i_EXAM_MANUAL_2,
	i_EXAM_MANUAL_3,
	i_EXAM_MANUAL_4,
	i_EXAM_MANUAL_5,
	i_EXAM_FTRANDOM_MIN,
	i_EXAM_FTRANDOM_MAX,
	s_APPEARTIME,
	f_SPEED_VALUE,
	END
}

public enum STAGE_LEVEL_4
{
	i_LINE_ID,
	i_EXAM_NUM,
	i_EXAM_MANUAL_1,
	i_EXAM_MANUAL_2,
	i_EXAM_MANUAL_3,
	i_EXAM_MANUAL_4,
	i_EXAM_MANUAL_5,
	i_EXAM_FTRANDOM_MIN,
	i_EXAM_FTRANDOM_MAX,
	s_APPEARTIME,
	f_SPEED_VALUE,
	END
}

public enum STAGE_LEVEL_40
{
	i_LINE_ID,
	i_EXAM_NUM,
	i_EXAM_MANUAL_1,
	i_EXAM_MANUAL_2,
	i_EXAM_MANUAL_3,
	i_EXAM_MANUAL_4,
	i_EXAM_MANUAL_5,
	i_EXAM_FTRANDOM_MIN,
	i_EXAM_FTRANDOM_MAX,
	s_APPEARTIME,
	f_SPEED_VALUE,
	END
}

public enum STAGE_LEVEL_41
{
	i_LINE_ID,
	i_EXAM_NUM,
	i_EXAM_MANUAL_1,
	i_EXAM_MANUAL_2,
	i_EXAM_MANUAL_3,
	i_EXAM_MANUAL_4,
	i_EXAM_MANUAL_5,
	i_EXAM_FTRANDOM_MIN,
	i_EXAM_FTRANDOM_MAX,
	s_APPEARTIME,
	f_SPEED_VALUE,
	END
}

public enum STAGE_LEVEL_42
{
	i_LINE_ID,
	i_EXAM_NUM,
	i_EXAM_MANUAL_1,
	i_EXAM_MANUAL_2,
	i_EXAM_MANUAL_3,
	i_EXAM_MANUAL_4,
	i_EXAM_MANUAL_5,
	i_EXAM_FTRANDOM_MIN,
	i_EXAM_FTRANDOM_MAX,
	s_APPEARTIME,
	f_SPEED_VALUE,
	END
}

public enum STAGE_LEVEL_43
{
	i_LINE_ID,
	i_EXAM_NUM,
	i_EXAM_MANUAL_1,
	i_EXAM_MANUAL_2,
	i_EXAM_MANUAL_3,
	i_EXAM_MANUAL_4,
	i_EXAM_MANUAL_5,
	i_EXAM_FTRANDOM_MIN,
	i_EXAM_FTRANDOM_MAX,
	s_APPEARTIME,
	f_SPEED_VALUE,
	END
}

public enum STAGE_LEVEL_44
{
	i_LINE_ID,
	i_EXAM_NUM,
	i_EXAM_MANUAL_1,
	i_EXAM_MANUAL_2,
	i_EXAM_MANUAL_3,
	i_EXAM_MANUAL_4,
	i_EXAM_MANUAL_5,
	i_EXAM_FTRANDOM_MIN,
	i_EXAM_FTRANDOM_MAX,
	s_APPEARTIME,
	f_SPEED_VALUE,
	END
}

public enum STAGE_LEVEL_45
{
	i_LINE_ID,
	i_EXAM_NUM,
	i_EXAM_MANUAL_1,
	i_EXAM_MANUAL_2,
	i_EXAM_MANUAL_3,
	i_EXAM_MANUAL_4,
	i_EXAM_MANUAL_5,
	i_EXAM_FTRANDOM_MIN,
	i_EXAM_FTRANDOM_MAX,
	s_APPEARTIME,
	f_SPEED_VALUE,
	END
}

public enum STAGE_LEVEL_46
{
	i_LINE_ID,
	i_EXAM_NUM,
	i_EXAM_MANUAL_1,
	i_EXAM_MANUAL_2,
	i_EXAM_MANUAL_3,
	i_EXAM_MANUAL_4,
	i_EXAM_MANUAL_5,
	i_EXAM_FTRANDOM_MIN,
	i_EXAM_FTRANDOM_MAX,
	s_APPEARTIME,
	f_SPEED_VALUE,
	END
}

public enum STAGE_LEVEL_47
{
	i_LINE_ID,
	i_EXAM_NUM,
	i_EXAM_MANUAL_1,
	i_EXAM_MANUAL_2,
	i_EXAM_MANUAL_3,
	i_EXAM_MANUAL_4,
	i_EXAM_MANUAL_5,
	i_EXAM_FTRANDOM_MIN,
	i_EXAM_FTRANDOM_MAX,
	s_APPEARTIME,
	f_SPEED_VALUE,
	END
}

public enum STAGE_LEVEL_48
{
	i_LINE_ID,
	i_EXAM_NUM,
	i_EXAM_MANUAL_1,
	i_EXAM_MANUAL_2,
	i_EXAM_MANUAL_3,
	i_EXAM_MANUAL_4,
	i_EXAM_MANUAL_5,
	i_EXAM_FTRANDOM_MIN,
	i_EXAM_FTRANDOM_MAX,
	s_APPEARTIME,
	f_SPEED_VALUE,
	END
}

public enum STAGE_LEVEL_49
{
	i_LINE_ID,
	i_EXAM_NUM,
	i_EXAM_MANUAL_1,
	i_EXAM_MANUAL_2,
	i_EXAM_MANUAL_3,
	i_EXAM_MANUAL_4,
	i_EXAM_MANUAL_5,
	i_EXAM_FTRANDOM_MIN,
	i_EXAM_FTRANDOM_MAX,
	s_APPEARTIME,
	f_SPEED_VALUE,
	END
}

public enum STAGE_LEVEL_5
{
	i_LINE_ID,
	i_EXAM_NUM,
	i_EXAM_MANUAL_1,
	i_EXAM_MANUAL_2,
	i_EXAM_MANUAL_3,
	i_EXAM_MANUAL_4,
	i_EXAM_MANUAL_5,
	i_EXAM_FTRANDOM_MIN,
	i_EXAM_FTRANDOM_MAX,
	s_APPEARTIME,
	f_SPEED_VALUE,
	END
}

public enum STAGE_LEVEL_50
{
	i_LINE_ID,
	i_EXAM_NUM,
	i_EXAM_MANUAL_1,
	i_EXAM_MANUAL_2,
	i_EXAM_MANUAL_3,
	i_EXAM_MANUAL_4,
	i_EXAM_MANUAL_5,
	i_EXAM_FTRANDOM_MIN,
	i_EXAM_FTRANDOM_MAX,
	s_APPEARTIME,
	f_SPEED_VALUE,
	END
}

public enum STAGE_LEVEL_6
{
	i_LINE_ID,
	i_EXAM_NUM,
	i_EXAM_MANUAL_1,
	i_EXAM_MANUAL_2,
	i_EXAM_MANUAL_3,
	i_EXAM_MANUAL_4,
	i_EXAM_MANUAL_5,
	i_EXAM_FTRANDOM_MIN,
	i_EXAM_FTRANDOM_MAX,
	s_APPEARTIME,
	f_SPEED_VALUE,
	END
}

public enum STAGE_LEVEL_7
{
	i_LINE_ID,
	i_EXAM_NUM,
	i_EXAM_MANUAL_1,
	i_EXAM_MANUAL_2,
	i_EXAM_MANUAL_3,
	i_EXAM_MANUAL_4,
	i_EXAM_MANUAL_5,
	i_EXAM_FTRANDOM_MIN,
	i_EXAM_FTRANDOM_MAX,
	s_APPEARTIME,
	f_SPEED_VALUE,
	END
}

public enum STAGE_LEVEL_8
{
	i_LINE_ID,
	i_EXAM_NUM,
	i_EXAM_MANUAL_1,
	i_EXAM_MANUAL_2,
	i_EXAM_MANUAL_3,
	i_EXAM_MANUAL_4,
	i_EXAM_MANUAL_5,
	i_EXAM_FTRANDOM_MIN,
	i_EXAM_FTRANDOM_MAX,
	s_APPEARTIME,
	f_SPEED_VALUE,
	END
}

public enum STAGE_LEVEL_9
{
	i_LINE_ID,
	i_EXAM_NUM,
	i_EXAM_MANUAL_1,
	i_EXAM_MANUAL_2,
	i_EXAM_MANUAL_3,
	i_EXAM_MANUAL_4,
	i_EXAM_MANUAL_5,
	i_EXAM_FTRANDOM_MIN,
	i_EXAM_FTRANDOM_MAX,
	s_APPEARTIME,
	f_SPEED_VALUE,
	END
}

public enum TITLE_MARK
{
	i_ID,
	s_TITLE_NAME_KR,
	s_TITLE_NAME_US,
	s_GRADE_NAME_KR,
	s_GRADE_NAME_US,
	s_RESOURCE,
	s_CONDITION,
	i_REWARD_ID,
	i_REWARD_AMOUNT,
	END
}
