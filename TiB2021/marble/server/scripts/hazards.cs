//-----------------------------------------------------------------------------
// Torque Game Engine
//
// Copyright (c) 2001 GarageGames.Com
// Portions Copyright (c) 2001 by Sierra Online, Inc.
//-----------------------------------------------------------------------------


//-----------------------------------------------------------------------------

datablock AudioProfile(TrapDoorOpenSfx)
{
   filename    = "~/data/sound/TrapDoorOpen.wav";
   description = AudioDefault3d;
   preload = true;
};


datablock StaticShapeData(TrapDoor)
{
   className = "Trap";
   category = "Hazards";
   shapeFile = "~/data/shapes/hazards/trapdoor.dts";
   resetTime = 5000;
   scopeAlways = true;
};

function TrapDoor::onAdd(%this, %obj)
{
   %obj.open = false;
   %obj.timeout = 200;
   if (%obj.resetTime $= "")
      %obj.resetTime = "Default";
}

function TrapDoor::onCollision(%this,%obj,%col)
{
   if (!%obj.open) {
      // pause before opening - give marble a chance to get off
      %this.schedule(%obj.timeout,"open",%obj);
      %obj.open = true;

      // Schedule the button reset
      %resetTime = (%obj.resetTime $= "Default")? %this.resetTime: %obj.resetTime;
      if (%resetTime)
         %this.schedule(%resetTime,close,%obj);
   }
}

function Trapdoor::open(%this, %obj)
{
   %obj.setThreadDir(0,true);
   %obj.playThread(0,"fall",1);
   %obj.playAudio(0,TrapDoorOpenSfx);
   %obj.open = true;
}

function Trapdoor::close(%this, %obj)
{
   %obj.setThreadDir(0,false);
   %obj.playAudio(0,TrapDoorOpenSfx);
   %obj.open = false;
}


//-----------------------------------------------------------------------------
datablock AudioProfile(DuctFanSfx)
{
   filename    = "~/data/sound/Fan_loop.wav";
   description = AudioClosestLooping3d;
   preload = true;
};


datablock StaticShapeData(DuctFan)
{
   className = "Fan";
   category = "Hazards";
   shapeFile = "~/data/shapes/hazards/ductfan.dts";
   scopeAlways = true;

   forceType[0] = Cone;       // Force type {Spherical, Field, Cone}
   forceNode[0] = 0;          // Shape node transform
   forceStrength[0] = 40;     // Force to apply
   forceRadius[0] = 10;       // Max radius
   forceArc[0] = 0.7;         // Cos angle

   powerOn = true;         // Default state
};

datablock StaticShapeData(SmallDuctFan)
{
   className = "Fan";
   category = "Hazards";
   shapeFile = "~/data/shapes/hazards/ductfan.dts";
   scopeAlways = true;

   scale = "0.5 0.5 0.5";

   forceType[0] = Cone;       // Force type {Spherical, Field, Cone}
   forceNode[0] = 0;          // Shape node transform
   forceStrength[0] = 10;     // Force to apply
   forceRadius[0] = 5;       // Max radius
   forceArc[0] = 0.7;         // Cos angle

   powerOn = true;         // Default state
};

function DuctFan::onAdd(%this,%obj)
{
   if (%this.powerOn)
   {
      %obj.playAudio(0, DuctFanSfx);
      %obj.playThread(0,"spin");
   }
   %obj.setPoweredState(%this.powerOn);
}

function DuctFan::onTrigger(%this,%obj,%mesg)
{
   if (%mesg)
   {
      %obj.playAudio(0, DuctFanSfx);
      %obj.playThread(0,"spin");
   }
   else
   {
      %obj.stopThread(0);
      %obj.stopAudio(0);
   }
   %obj.setPoweredState(%mesg);
}

function SmallDuctFan::onAdd(%this,%obj)
{
   if (%this.powerOn)
   {
      %obj.playThread(0,"spin");
      %obj.playAudio(0, DuctFanSfx);
   }
   %obj.setPoweredState(%this.powerOn);
}

function SmallDuctFan::onTrigger(%this,%obj,%mesg)
{
   if (%mesg)
   {
      %obj.playAudio(0, DuctFanSfx);
      %obj.playThread(0,"spin");
   }
   else
   {
      %obj.stopThread(0);
      %obj.stopAudio(0);
   }

   %obj.setPoweredState(%mesg);
}


//-----------------------------------------------------------------------------

datablock StaticShapeData(OilSlick)
{
   category = "Hazards";
   shapeFile = "~/data/shapes/hazards/oilslick.dts";
   scopeAlways = true;
};


//-----------------------------------------------------------------------------

datablock AudioProfile(TornadoSfx)
{
   filename    = "~/data/sound/Tornado.wav";
   description = AudioClosestLooping3d;
   preload = true;
};

datablock StaticShapeData(Tornado)
{
   category = "Hazards";
   shapeFile = "~/data/shapes/hazards/tornado.dts";
   scopeAlways = true;

   // Pull the marble in
   forceType[0] = Spherical;  // Force type {Spherical, Field, Cone}
   forceStrength[0] = -60;     // Force to apply
   forceRadius[0] = 8;       // Max radius

   // Counter sphere to slow the marble down near the center
   forceType[1] = Spherical;
   forceStrength[1] = 60;
   forceRadius[1] = 3;

   // Field to shoot the marble up
   forceType[2] = Field;
   forceVector[2] = "0 0 1";
   forceStrength[2] = 250;
   forceRadius[2] = 3;
};

function Tornado::onAdd(%this,%obj)
{
   %obj.playThread(0,"ambient");
   %obj.playAudio(0,TornadoSfx);
   %obj.setPoweredState(true);
}

//-----------------------------------------------------------------------------
// LandMine

datablock AudioProfile(ExplodeSfx)
{
   filename    = "~/data/sound/explode1.wav";
   description = AudioDefault3d;
   preload = true;
};

datablock ParticleData(LandMineParticle)
{
   textureName          = "~/data/particles/smoke";
   dragCoefficient      = 2;
   gravityCoefficient   = 0.2;
   inheritedVelFactor   = 0.2;
   constantAcceleration = 0.0;
   lifetimeMS           = 1000;
   lifetimeVarianceMS   = 150;

   colors[0]     = "0.56 0.36 0.26 1.0";
   colors[1]     = "0.56 0.36 0.26 0.0";

   sizes[0]      = 0.5;
   sizes[1]      = 1.0;
};

datablock ParticleEmitterData(LandMineEmitter)
{
   ejectionPeriodMS = 7;
   periodVarianceMS = 0;
   ejectionVelocity = 2;
   velocityVariance = 1.0;
   ejectionOffset   = 0.0;
   thetaMin         = 0;
   thetaMax         = 60;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvances = false;
   particles = "LandMineParticle";
};

datablock ParticleData(LandMineSmoke)
{
   textureName          = "~/data/particles/smoke";
   dragCoeffiecient     = 100.0;
   gravityCoefficient   = 0;
   inheritedVelFactor   = 0.25;
   constantAcceleration = -0.80;
   lifetimeMS           = 1200;
   lifetimeVarianceMS   = 300;
   useInvAlpha =  true;
   spinRandomMin = -80.0;
   spinRandomMax =  80.0;

   colors[0]     = "0.56 0.36 0.26 1.0";
   colors[1]     = "0.2 0.2 0.2 1.0";
   colors[2]     = "0.0 0.0 0.0 0.0";

   sizes[0]      = 1.0;
   sizes[1]      = 1.5;
   sizes[2]      = 2.0;

   times[0]      = 0.0;
   times[1]      = 0.5;
   times[2]      = 1.0;
};

datablock ParticleEmitterData(LandMineSmokeEmitter)
{
   ejectionPeriodMS = 10;
   periodVarianceMS = 0;
   ejectionVelocity = 4;
   velocityVariance = 0.5;
   thetaMin         = 0.0;
   thetaMax         = 180.0;
   lifetimeMS       = 250;
   particles = "LandMineSmoke";
};

datablock ParticleData(LandMineSparks)
{
   textureName          = "~/data/particles/spark";
   dragCoefficient      = 1;
   gravityCoefficient   = 0.0;
   inheritedVelFactor   = 0.2;
   constantAcceleration = 0.0;
   lifetimeMS           = 500;
   lifetimeVarianceMS   = 350;

   colors[0]     = "0.60 0.40 0.30 1.0";
   colors[1]     = "0.60 0.40 0.30 1.0";
   colors[2]     = "1.0 0.40 0.30 0.0";

   sizes[0]      = 0.5;
   sizes[1]      = 0.25;
   sizes[2]      = 0.25;

   times[0]      = 0.0;
   times[1]      = 0.5;
   times[2]      = 1.0;
};

datablock ParticleEmitterData(LandMineSparkEmitter)
{
   ejectionPeriodMS = 3;
   periodVarianceMS = 0;
   ejectionVelocity = 13;
   velocityVariance = 6.75;
   ejectionOffset   = 0.0;
   thetaMin         = 0;
   thetaMax         = 180;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvances = false;
   orientParticles  = true;
   lifetimeMS       = 100;
   particles = "LandMineSparks";
};

datablock ExplosionData(LandMineSubExplosion1)
{
   offset = 1.0;
   emitter[0] = LandMineSmokeEmitter;
   emitter[1] = LandMineSparkEmitter;
};

datablock ExplosionData(LandMineSubExplosion2)
{
   offset = 1.0;
   emitter[0] = LandMineSmokeEmitter;
   emitter[1] = LandMineSparkEmitter;
};

datablock ExplosionData(LandMineExplosion)
{
   soundProfile = ExplodeSfx;
   lifeTimeMS = 1200;

   // Volume particles
   particleEmitter = LandMineEmitter;
   particleDensity = 80;
   particleRadius = 1;

   // Point emission
   emitter[0] = LandMineSmokeEmitter;
   emitter[1] = LandMineSparkEmitter;

   // Sub explosion objects
   subExplosion[0] = LandMineSubExplosion1;
   subExplosion[1] = LandMineSubExplosion2;
   
   // Camera Shaking
   shakeCamera = true;
   camShakeFreq = "10.0 11.0 10.0";
   camShakeAmp = "1.0 1.0 1.0";
   camShakeDuration = 0.5;
   camShakeRadius = 10.0;

   // Impulse
   impulseRadius = 10;
   impulseForce = 15;

   // Dynamic light
   lightStartRadius = 6;
   lightEndRadius = 3;
   lightStartColor = "0.5 0.5 0";
   lightEndColor = "0 0 0";
};

datablock StaticShapeData(LandMine)
{
   className = "Explosive";
   category = "Hazards";
   shapeFile = "~/data/shapes/hazards/landmine.dts";
   explosion = LandMineExplosion;
   renderWhenDestroyed = false;
   resetTime = 5000;
};

function LandMine::onAdd(%this, %obj)
{
   if (%obj.resetTime $= "")
      %obj.resetTime = "Default";
}

function LandMine::onCollision(%this, %obj, %col)
{
   %obj.setDamageState("Destroyed");

   %resetTime = (%obj.resetTime $= "Default")? %this.resetTime: %obj.resetTime;
   if (%resetTime) {
      %obj.startFade(0, 0, true);
      %obj.schedule(%resetTime, setDamageState,"Enabled");
      %obj.schedule(%resetTime, "startFade", 1000, 0, false);
   }
}

//------------------------------------------------------------------------------------

//-----------------------------------------------------------------------------
// Nuuk

datablock AudioProfile(NuukSfx)
{
   filename    = "~/data/sound/explode1.wav";
   description = AudioDefault3d;
   preload = true;
};

datablock ParticleData(NuukParticle)
{
   textureName          = "~/data/particles/smoke";
   dragCoefficient      = 4;
   gravityCoefficient   = 1;
   inheritedVelFactor   = 1;
   constantAcceleration = 5.0;
   lifetimeMS           = 50000;
   lifetimeVarianceMS   = 5500;

   colors[0]     = "0.56 0.36 0.26 1.0";
   colors[1]     = "0.56 0.36 0.26 0.0";

   sizes[0]      = 2.0;
   sizes[1]      = 4.0;
};

datablock ParticleEmitterData(NuukEmitter)
{
   ejectionPeriodMS = 54;
   periodVarianceMS = 5;
   ejectionVelocity = 5;
   velocityVariance = 5.0;
   ejectionOffset   = 5.0;
   thetaMin         = 5;
   thetaMax         = 520;
   phiReferenceVel  = 5;
   phiVariance      = 560;
   overrideAdvances = false;
   particles = "NuukParticle";
};

datablock ParticleData(NuukSmoke)
{
   textureName          = "~/data/particles/smoke";
   dragCoeffiecient     = 500.0;
   gravityCoefficient   = 5;
   inheritedVelFactor   = 5.25;
   constantAcceleration = -5.80;
   lifetimeMS           = 5200;
   lifetimeVarianceMS   = 500;
   useInvAlpha =  true;
   spinRandomMin = -50.0;
   spinRandomMax =  50.0;

   colors[0]     = "0.56 0.36 0.26 1.0";
   colors[1]     = "0.2 0.2 0.2 1.0";
   colors[2]     = "0.0 0.0 0.0 0.0";

   sizes[0]      = 5.0;
   sizes[1]      = 5.5;
   sizes[2]      = 5.0;

   times[0]      = 5.0;
   times[1]      = 5.5;
   times[2]      = 5.0;
};

datablock ParticleEmitterData(NuukSmokeEmitter)
{
   ejectionPeriodMS = 50;
   periodVarianceMS = 5;
   ejectionVelocity = 5;
   velocityVariance = 5.0;
   thetaMin         = 5.0;
   thetaMax         = 580.0;
   lifetimeMS       = 550;
   particles = "NuukSmoke";
};

datablock ParticleData(NuukSparks)
{
   textureName          = "~/data/particles/spark";
   dragCoefficient      = 5;
   gravityCoefficient   = 5.0;
   inheritedVelFactor   = 5.2;
   constantAcceleration = 5.0;
   lifetimeMS           = 5000;
   lifetimeVarianceMS   = 1050;

   colors[0]     = "0.60 0.40 0.30 1.0";
   colors[1]     = "0.60 0.40 0.30 1.0";
   colors[2]     = "1.0 0.40 0.30 0.0";

   sizes[0]      = 5.5;
   sizes[1]      = 5.25;
   sizes[2]      = 5.25;

   times[0]      = 5.0;
   times[1]      = 5.5;
   times[2]      = 5.0;
};

datablock ParticleEmitterData(NuukSparkEmitter)
{
   ejectionPeriodMS = 12;
   periodVarianceMS = 12;
   ejectionVelocity = 50;
   velocityVariance = 12.75;
   ejectionOffset   = 12.0;
   thetaMin         = 12;
   thetaMax         = 6280;
   phiReferenceVel  = 22;
   phiVariance      = 5260;
   overrideAdvances = false;
   orientParticles  = true;
   lifetimeMS       = 12000;
   particles = "NuukSparks";
};

datablock ExplosionData(NuukSubExplosion1)
{
   offset = 122.0;
   emitter[0] = LandMineSmokeEmitter;
   emitter[1] = LandMineSparkEmitter;
};

datablock ExplosionData(NuukSubExplosion2)
{
   offset = 122.0;
   emitter[0] = LandMineSmokeEmitter;
   emitter[1] = LandMineSparkEmitter;
};

datablock ExplosionData(NuukExplosion)
{
   soundProfile = ExplodeSfx;
   lifeTimeMS = 50200;

   // Volume particles
   particleEmitter = NuukEmitter;
   particleDensity = 820;
   particleRadius = 220;

   // Point emission
   emitter[0] = NuukSmokeEmitter;
   emitter[1] = NuukSparkEmitter;

   // Sub explosion objects
   subExplosion[0] = NuukSubExplosion1;
   subExplosion[1] = NuukSubExplosion2;
   
   // Camera Shaking
   shakeCamera = true;
   camShakeFreq = "10.0 11.0 10.0";
   camShakeAmp = "1.0 1.0 1.0";
   camShakeDuration = 5.0;
   camShakeRadius = 50.0;

   // Impulse
   impulseRadius = 50;
   impulseForce = 100;

   // Dynamic light
   lightStartRadius = 36;
   lightEndRadius = 12;
   lightStartColor = "0.5 0.5 0";
   lightEndColor = "0 0 0";
};

datablock StaticShapeData(Nuuk)
{
   className = "Explosive";
   category = "Hazards";
   shapeFile = "~/data/shapes/hazards/moonmine/landmine.dts";
   explosion = NuukExplosion;
   renderWhenDestroyed = false;
   resetTime = 1000;
};

function Nuuk::onAdd(%this, %obj)
{
   if (%obj.resetTime $= "")
      %obj.resetTime = "Default";
}

function Nuuk::onCollision(%this, %obj, %col)
{
   %obj.setDamageState("Destroyed");

   %resetTime = (%obj.resetTime $= "Default")? %this.resetTime: %obj.resetTime;
   if (%resetTime) {
      %obj.startFade(0, 0, true);
      %obj.schedule(%resetTime, setDamageState,"Enabled");
      %obj.schedule(%resetTime, "startFade", 1000, 0, false);
   }
}

//------------------------------------------------------------------------------------

datablock AudioProfile(BLACKHOLESfx)
{
   filename    = "~/data/sound/Tfornado.wav";
   description = AudioClosestLooping3d;
   preload = true;
};

datablock StaticShapeData(BLACKHOLE)
{
   category = "Hazards";
   shapeFile = "~/data/shapes/hazards/BLACKHOLE/BLACKHOLE.dts";
   scopeAlways = true;

   // Pull the marble in
   forceType[0] = Spherical;  // Force type {Spherical, Field, Cone}
   forceStrength[0] = -99999;     // Force to apply
   forceRadius[0] = 8;       // Max radius

   // Counter sphere to slow the marble down near the center
   forceType[1] = Spherical;
   forceStrength[1] = 60;
   forceRadius[1] = 3;

   // Field to shoot the marble up
   forceType[2] = Field;
   forceVector[2] = "0 0 1";
   forceStrength[2] = -99999;
   forceRadius[2] = 3;
};

function BLACKHOLE::onAdd(%this,%obj)
{
   %obj.playThread(0,"ambient");
   %obj.playAudio(0,BLACKHOLESfx);
   %obj.setPoweredState(true);
}

//-----------------------------------------------------------------------------

datablock AudioProfile(SUNSfx)
{
   filename    = "~/data/sound/TfornSUNado.wav";
   description = AudioClosestLooping3d;
   preload = true;
};

datablock StaticShapeData(SUN)
{
   category = "Hazards";
   shapeFile = "~/data/shapes/hazards/SUN/SUN.dts";
   scopeAlways = true;

   // Pull the marble in
   forceType[0] = Spherical;  // Force type {Spherical, Field, Cone}
   forceStrength[0] = -250;     // Force to apply
   forceRadius[0] = 8;       // Max radius

   // Counter sphere to slow the marble down near the center
   forceType[1] = Spherical;
   forceStrength[1] = 60;
   forceRadius[1] = 3;

   // Field to shoot the marble up
   forceType[2] = Field;
   forceVector[2] = "0 0 1";
   forceStrength[2] = -250;
   forceRadius[2] = 3;
};

function SUN::onAdd(%this,%obj)
{
   %obj.playThread(0,"ambient");
   %obj.playAudio(0,SUNSfx);
   %obj.setPoweredState(true);
}

//-----------------------------------------------------------------------------

datablock AudioProfile(EARTHSfx)
{
   filename    = "~/data/sound/TfornSEARTHUNado.wav";
   description = AudioClosestLooping3d;
   preload = true;
};

datablock StaticShapeData(EARTH)
{
   category = "Hazards";
   shapeFile = "~/data/shapes/hazards/EARTH/EARTH.dts";
   scopeAlways = true;

   // Pull the marble in
   forceType[0] = Spherical;  // Force type {Spherical, Field, Cone}
   forceStrength[0] = -99;     // Force to apply
   forceRadius[0] = 8;       // Max radius

   // Counter sphere to slow the marble down near the center
   forceType[1] = Spherical;
   forceStrength[1] = 60;
   forceRadius[1] = 3;

   // Field to shoot the marble up
   forceType[2] = Field;
   forceVector[2] = "0 0 1";
   forceStrength[2] = -250;
   forceRadius[2] = 3;
};

function EARTH::onAdd(%this,%obj)
{
   %obj.playThread(0,"ambient");
   %obj.playAudio(0,EARTHSfx);
   %obj.setPoweredState(true);
}

//-----------------------------------------------------------------------------

datablock AudioProfile(PushMarbleSfx)
{
   filename    = "~/data/sound/forcefield.wav";
   description = AudioClosestLooping3d;
   preload = true;
};

datablock StaticShapeData(PushMarble)
{
   category = "Hazards";
   shapeFile = "~/data/shapes/images/glow_bounce.dts";
   scopeAlways = true;
  

   // Pull the marble in
   forceType[0] = Spherical;  // Force type {Spherical, Field, Cone}
   forceStrength[0] = 99;     // Force to apply
   forceRadius[0] = 8;       // Max radius

   // Counter sphere to slow the marble down near the center
   forceType[1] = Spherical;
   forceStrength[1] = 60;
   forceRadius[1] = 3;

   // Field to shoot the marble up
   forceType[2] = Field;
   forceVector[2] = "0 0 1";
   forceStrength[2] = 250;
   forceRadius[2] = 3;
};

function PushMarble::onAdd(%this,%obj)
{
   %obj.playThread(0,"ambient");
   %obj.playAudio(0,PushMarbleSfx);
   %obj.setPoweredState(true);
}

//-----------------------------------------------------------------------------

