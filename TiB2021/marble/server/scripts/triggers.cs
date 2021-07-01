//-----------------------------------------------------------------------------
// Torque Game Engine
// 
// Copyright (c) 2001 GarageGames.Com
// Portions Copyright (c) 2001 by Sierra Online, Inc.
//-----------------------------------------------------------------------------

// Normally the trigger class would be sub-classed to support different
// functionality, but we're going to use the name of the trigger instead
// as an experiment.

//-----------------------------------------------------------------------------

datablock TriggerData(InBoundsTrigger)
{
   tickPeriodMS = 100;
};

function InBoundsTrigger::onLeaveTrigger(%this,%trigger,%obj)
{
   // Leaving an in bounds area.
   %obj.getDatablock().onOutOfBounds(%obj);
}


//-----------------------------------------------------------------------------

datablock TriggerData(OutOfBoundsTrigger)
{
   tickPeriodMS = 100;
};

function OutOfBoundsTrigger::onEnterTrigger(%this,%trigger,%obj)
{
   // Entering an out of bounds area
   %obj.getDatablock().onOutOfBounds(%obj);
}

//-----------------------------------------------------------------------------

datablock TriggerData(HelpTrigger)
{
   tickPeriodMS = 100;
};

function HelpTrigger::onEnterTrigger(%this,%trigger,%obj)
{
   // Leaving an in bounds area.
   addHelpLine(%trigger.text, true);
}


//-----------------------------------------------------------------------------
//By Jeff

datablock TriggerData(DoubleJumpTrigger)
{
tickPeriodMS = 100;
};

function DoubleJumpTrigger::onEnterTrigger(%this,%trigger,%obj)
{
defaultmarble.jumpimpulse=15;
}

function DoubleJumpTrigger::onLeaveTrigger(%this,%trigger,%obj)
{
defaultmarble.jumpimpulse=7.5;
}

//-----------------------------------------------------------------------------
//By Technostar (Template By Jeff)

datablock TriggerData(ZeroGravityTrigger)
{
tickPeriodMS = 100;
};

function ZeroGravityTrigger::onEnterTrigger(%this,%trigger,%obj)
{
defaultmarble.gravity=0;
}


//-----------------------------------------------------------------------------
//By Technostar (Template By Jeff)

datablock TriggerData(LowGravityTrigger)
{
tickPeriodMS = 100;
};

function LowGravityTrigger::onEnterTrigger(%this,%trigger,%obj)
{
defaultmarble.gravity=10;
}


//-----------------------------------------------------------------------------
//By Technostar (Template By Jeff)

datablock TriggerData(NormGravityTrigger)
{
tickPeriodMS = 100;
};

function NormGravityTrigger::onEnterTrigger(%this,%trigger,%obj)
{
defaultmarble.gravity=20;
}


//-----------------------------------------------------------------------------
//By Technostar (Template By Jeff)

datablock TriggerData(ZeroGravityTrigger)
{
tickPeriodMS = 100;
};

function ZeroGravityTrigger::onEnterTrigger(%this,%trigger,%obj)
{
defaultmarble.gravity=0;
}


//-----------------------------------------------------------------------------
//By Technostar (Template By Jeff)

datablock TriggerData(LowGravityTrigger)
{
tickPeriodMS = 100;
};

function LowGravityTrigger::onEnterTrigger(%this,%trigger,%obj)
{
defaultmarble.gravity=10;
}


//-----------------------------------------------------------------------------
//By Technostar (Template By Jeff)

datablock TriggerData(NormGravityTrigger)
{
tickPeriodMS = 100;
};

function NormGravityTrigger::onEnterTrigger(%this,%trigger,%obj)
{
defaultmarble.gravity=20;
}


//-----------------------------------------------------------------------------
//By Technostar (Template By Jeff)

datablock TriggerData(HighGravityTrigger)
{
tickPeriodMS = 100;
};

function HighGravityTrigger::onEnterTrigger(%this,%trigger,%obj)
{
defaultmarble.gravity=40;
}

//-----------------------------------------------------------------------------
//By Technostar (Template By Jeff)

datablock TriggerData(LowGravityTrigger)
{
tickPeriodMS = 100;
};

function LowGravityTrigger::onEnterTrigger(%this,%trigger,%obj)
{
defaultmarble.gravity=10;
}

//-----------------------------------------------------------------------------
//By Technostar (Template By Jeff)

datablock TriggerData(NormGravityTrigger)
{
tickPeriodMS = 100;
};

function NormGravityTrigger::onEnterTrigger(%this,%trigger,%obj)
{
defaultmarble.gravity=20;
}


//-----------------------------------------------------------------------------
//By Technostar (Template By Jeff)

datablock TriggerData(NormGravityTrigger)
{
tickPeriodMS = 100;
};

function NormGravityTrigger::onEnterTrigger(%this,%trigger,%obj)
{
defaultmarble.gravity=20;
}

//-----------------------------------------------------------------------------
//By Technostar (Template By Jeff)

datablock TriggerData(HighGravityTrigger)
{
tickPeriodMS = 100;
};

function HighGravityTrigger::onEnterTrigger(%this,%trigger,%obj)
{
defaultmarble.gravity=40;
}
