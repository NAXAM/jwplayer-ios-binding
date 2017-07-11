using System;
using ObjCRuntime;

namespace JWPlayer
{
	[Native]
	public enum JWAdCompanionType : long
	{
		Static,
		Iframe,
		Html
	}

	public enum JWAdClient : ulong
	{
		vastPlugin = 1,
		googIMA = 2
	}

	public enum JWEdgeStyle : ulong
	{
		none,
		dropshadow,
		raised,
		depressed,
		uniform
	}

	public enum JWCastingService : ulong
	{
		googleChromeCast = 1
	}

	public enum JWRelatedCompleteAction : ulong
	{
		show = 0,
		hide,
		autoplay
	}

	public enum JWRelatedOpenMethod : ulong
	{
		api = 0,
		complete,
		click
	}

	public enum JWPremiumSkin : ulong
	{
		Seven = 0,
		Beelden,
		Bekle,
		Five,
		Glow,
		Roundster,
		Stormtrooper,
		Vapor,
		Six
	}

	public enum JWStretching : ulong
	{
		Uniform = 0,
		ExactFit,
		Fill,
		None
	}

	public enum JWEncryption : ulong
	{
		JWFairPlay = 0
	}
}
