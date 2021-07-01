datablock StaticShapeData(CheckpointPad) {
   className = "CheckPoint";
   category = "Pads";
   shapeFile = "~/data/shapes/pads/checkpoint.dts";
   scopeAlways = true;
   emap = false;
};

function GameConnection::setCheckPoint(%this, %pad) {
   //echo("Setting checkpoint to" SPC %pad);
   //echo("LastCheckpoint is" SPC %this.lastCheckpoint);
   //echo("Checkpoint is" SPC %this.checkpoint);
   if (%this.lastCheckpoint != %pad) {
      echo("New CP!");
      %this.checkpoint = %pad;
      %this.lastCheckpoint = %pad;
      %this.setCheckPointPowerUp(%this.player.getPowerUp());
      %this.setCheckPointGemCount(%this.gemCount);
      %this.setCheckPointPenaltyTime(0); //Really?
      %this.setCheckPointBonusTime(PlayGui.bonusTime);
      %this.setCheckPointTime(PlayGui.time); //Should really make this ClientConnection Compatible... Oh well
      %this.setCheckPointPad(%pad);
      %this.setCheckPointGravityDir($Game::GravityDir);
      %this.setCheckPointGems();
      if (%pad != 0) {
         messageClient(%this, 'MsgItemPickup', '\c0CheckPoint reached!');
         %this.play2d(checkpointSfx);
      }
   }
   //echo("Pad is object:" SPC %this.getCheckpointPad());
}

function GameConnection::hasCheckPoint(%this) {
   return (%this.checkpoint ? true : false);
}

//Getters
function GameConnection::getCheckPointPowerUp(%this) {
   return (%this.checkpoint ? (%this.checkpointpowerup ? %this.checkpointpowerup : 0) : 0);
}
function GameConnection::getCheckPointGemCount(%this) {
   return (%this.checkpoint ? (%this.checkpointgemCount ? %this.checkpointgemCount : 0) : 0);
}
function GameConnection::getCheckPointPenaltyTime(%this) {
   return (%this.checkpoint ? (%this.checkpointpenaltyTime ? %this.checkpointpenaltyTime : 0) : 0);
}
function GameConnection::getCheckPointBonusTime(%this) {
   return (%this.checkpoint ? (%this.checkpointbonusTime ? %this.checkpointbonusTime : 0) : 0);
}
function GameConnection::getCheckPointTime(%this) {
   return (%this.checkpoint ? (%this.checkpointtime ? %this.checkpointtime : 0) : 0);
}
function GameConnection::getCheckPointPad(%this) {
   return (%this.checkpoint ? (%this.checkpointpad ? %this.checkpointpad : 0) : 0); //Usually just the CP pad itself
}
function GameConnection::getCheckPointGravityDir(%this) {
   return (%this.checkpoint ? (%this.checkpointgravityDir ? %this.checkpointgravityDir : 0) : "1 0 0 0 -1 0 0 0 -1");
}

//Setters
function GameConnection::setCheckPointPowerUp(%this, %powerup) {
   if (%this.checkpoint)
      %this.checkpointpowerup = %powerup;
}
function GameConnection::setCheckPointGemCount(%this, %gemCount) {
   if (%this.checkpoint)
      %this.checkpointgemCount = %gemCount;
}
function GameConnection::setCheckPointPenaltyTime(%this, %penaltyTime) {
   if (%this.checkpoint)
      %this.checkpointpenaltyTime = %penaltyTime;
}
function GameConnection::setCheckPointBonusTime(%this, %bonusTime) {
   if (%this.checkpoint)
      %this.checkpointbonusTime = %bonusTime;
}
function GameConnection::setCheckPointTime(%this, %time) {
   if (%this.checkpoint)
      %this.checkpointtime = %time;
}
function GameConnection::setCheckPointPad(%this, %pad) {
   if (%this.checkpoint)
      %this.checkpointpad = %pad;
}
function GameConnection::setCheckPointGravityDir(%this, %gravityDir) {
   if (%this.checkpoint)
      %this.checkpointgravityDir = %gravityDir;
}

//Gems
function GameConnection::setCheckPointGems(%this) {
   makeGemGroup(MissionGroup, true);
   %this.gemsCollected = "";
   for (%i = 0; %i < $GemsCount; %i ++) {
      %gem = $Gems[%i];
      if (%gem.isHidden()) {
         %this.gemsCollected = %this.gemsCollected SPC %gem;
      }
   }
}

function GameConnection::respawnCheckPointGems(%this) {
   makeGemGroup(MissionGroup, true);
   for (%i = 0; %i < $GemsCount; %i ++) {
      %gem = $Gems[%i];
      %gem.hide(false);
   }
   for (%i = 0; %i < getWordCount(%this.gemsCollected); %i ++) {
      %gem = getWord(%this.gemsCollected, %i);
      %gem.hide(true);
   }
}

//Gravity Override
package GravityOverride {
   function setGravityDir(%dir, %idk) {
      doSetGravityDir(%dir, %idk);
   }
};

function doSetGravityDir(%dir, %idk) {
   deactivatePackage(GravityOverride);
   setGravityDir(%dir, %idk);
   $Game::GravityDir = %dir;
   activatePackage(GravityOverride);
}

activatePackage(GravityOverride);