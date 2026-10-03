module PrototypeFighter.Scripts.Bot.Hurts

open FGScript
open PrototypeFighter.Scripts
open Macros

let states =
    def PARTIAL {
        def STATE s.HitStun.Stand.Legs {
            stance Stand
            category Hitstun
            updateDuringFrameStop
            condition (Args.HitStun.zone === HitZone.Legs)
            action { stopAnimate (Var.Entity.stunFrames + Var.stopFrames) a.HitStun.Stand.Legs 0 15 }
        }

        def STATE s.HitStun.Stand.Body {
            stance Stand
            category Hitstun
            updateDuringFrameStop
            condition (Args.HitStun.zone === HitZone.Body)
            action { stopAnimate (Var.Entity.stunFrames + Var.stopFrames) a.HitStun.Stand.Body 0 15 }
        }

        def STATE s.HitStun.Stand.Head {
            stance Stand
            category Hitstun
            updateDuringFrameStop
            condition (Args.HitStun.zone === HitZone.Head)
            action { stopAnimate (Var.Entity.stunFrames + Var.stopFrames) a.HitStun.Stand.Head 0 15 }
        }

        def STATE s.HitStun.Crouch {
            stance Crouch
            category Hitstun
            updateDuringFrameStop
            action { stopAnimate (Var.Entity.stunFrames + Var.stopFrames) a.HitStun.Crouch 0 15 }
        }

        def STATE s.HitStun.Crumple {
            stance Stand
            category Hitstun
            require Grounded
            updateDuringFrameStop
            meta Meta.HitStun.Special
            condition (Args.HitStun.flagged HitFlags.CausesCrumple)
            next s.HitStun.LandFront

            action {
                shakeFrame Var.stopFrames a.HitStun.Crumple 1 3<frames> 2.5
                animate a.HitStun.Crumple 2 15 2<times>
                animate a.HitStun.Crumple 16 45
                stance Airborne
                animate a.HitStun.Crumple 46 56
            }
        }

        def STATE s.HitStun.Stagger {
            stance Stand
            category Hitstun
            require Grounded
            updateDuringFrameStop
            meta Meta.HitStun.Special
            condition (Args.HitStun.flagged HitFlags.Stagger)

            let extraFrames = 24

            action {
                shakeFrame Var.stopFrames a.HitStun.Stagger 0 3<frames> 2.5
                fitAnimate (Var.Entity.stunFrames + extraFrames) a.HitStun.Stagger 1 40
            }
        }

        def STATE s.HitStun.LandBack {
            category KnockDown
            stance Airborne
            keepFacingSide
            next s.LyingDown.FaceUp (args Args.HitStun.hitFlags)

            action {
                On ActionBegin {
                    If(
                        Args.HitStun.flagged HitFlags.GroundBounce
                        .& !Query.LastState(s.HitStun.GroundBounce)
                    ) {
                        changeState s.HitStun.GroundBounce Args.Forward
                    }

                    clear Position.Y
                    clear Vel.Y
                }

                frame a.HitStun.KnockDown 12
                call "KnockLand" (args (Args.HitStun.flagged HitFlags.HardKnockDown))
                animate a.HitStun.KnockDown 13 20
            }
        }

        def STATE s.HitStun.LandFront {
            category KnockDown
            stance Airborne
            keepFacingSide
            next s.LyingDown.FaceDown (args Args.HitStun.hitFlags)

            action {
                On ActionBegin { clear Position.Y }
                frame a.HitStun.Crumple 57
                call "KnockLand" (args (Args.HitStun.flagged HitFlags.HardKnockDown))
                animate a.HitStun.Crumple 58 65
            }
        }

        def FUNC "KnockLand" {
            clear Acc
            clear Vel.X
            effect Vfx.Dust Floor [ Translate(Axis.X, -50); Rotate(Axis.Z, -15.<deg>) ]
            effect Vfx.Dust Floor [ Translate(Axis.X, 50); Rotate(Axis.Z, -165.<deg>) ]

            If(IsTrue Arg1) {
                notify Notifications.HardKnockDown
                playSound Sfx.KnockDown.Hard (Volume -6<Db>)
            }

            Else { playSound Sfx.KnockDown.Soft (Volume -6<Db>) }
        }

        def STATE s.HitStun.KnockDown {
            stance Stand
            category Hitstun
            updateDuringFrameStop
            pushDuringFrameStop
            meta Meta.HitStun.Special
            condition (Args.HitStun.flagged HitFlags.Sweep)
            next s.HitStun.LandBack Args.Forward

            action {
                On ActionBegin {
                    clear Position.Y
                    stop
                }

                stopAnimate Var.stopFrames a.HitStun.KnockDown 1 6
                animate a.HitStun.KnockDown 7 11
            }
        }

        def STATE s.HitStun.KnockBack {
            stance Airborne
            category Hitstun
            meta Meta.HitStun.Special
            condition (Args.HitStun.flagged HitFlags.KnockBack)
            next s.HitStun.LandBack Args.Forward

            action {
                On ActionBegin {
                    update Position.Y (Max(num 10))
                    set Var.Body.anchored
                }

                On TouchWall {
                    If(
                        Args.HitStun.flagged HitFlags.WallBounce
                        .& !Query.HasHitFlagsInCombo(HitFlags.WallBounce)
                    ) {
                        requestState s.HitStun.WallBounce
                    }

                    ElseIf(Args.HitStun.flagged HitFlags.WallSplat) { requestState s.HitStun.WallSplat }
                }

                If(Var.LastReceived.attackData != 0) { log "Custom hit value" Var.LastReceived.attackData }

                animate a.HitStun.KnockBack 1 4
                clear Var.Body.anchored
                On Landing { exit }
                fitAnimate Var.Body.currentApexFrames a.HitStun.KnockBack 5 24
                animate a.HitStun.KnockBack 25 44
                Loop { frame }
            }
        }

        def STATE s.HitStun.KnockUp {
            stance Airborne
            category Hitstun
            meta Meta.HitStun.Special
            condition (Args.HitStun.flagged HitFlags.KnockUp)
            next s.HitStun.LandBack Args.Forward

            action {
                On ActionBegin {
                    update Position.Y (Max(num 15))
                    set Var.Body.anchored
                }

                animate a.HitStun.KnockUp 1 4
                clear Var.Body.anchored
                On Landing { exit }
                fitAnimate Var.Body.currentApexFrames a.HitStun.KnockUp 5 24
                animate a.HitStun.KnockUp 25 44
                Loop { frame }
            }
        }

        def STATE s.HitStun.Air.Default {
            stance Airborne
            category Hitstun
            updateDuringFrameStop
            next s.Jump.Landing

            let stunTime = Calc.Min(Var.Entity.stunFrames, num 12) + Var.stopFrames

            action {
                On ActionBegin {
                    set Gravity p.Gravity
                    update Position.Y (Max(num 15))
                    set Vel (-2, 14)
                }

                On ActionEnd {
                    clear Gravity
                    clear Vel
                }

                If Var.Entity.isDead { changeState s.HitStun.Air.Die }

                fitAnimate stunTime a.HitStun.KnockBack 1 14
                On Landing { exit }
                animate Blend (a.HitStun.KnockBack, 14) a.HitStun.Air.Flipout 5 20
                animate a.HitStun.Air.Flipout 21 35
                Loop { frame a.HitStun.Air.Flipout 36 }
            }
        }

        def STATE s.HitStun.GroundBounce {
            stance Airborne
            category Hitstun
            skipMeta
            next s.HitStun.LandBack Args.Forward

            action {
                On ActionBegin {
                    stop
                    clear Position.Y
                    clear Vel.Y
                    clear Gravity
                }

                frame a.HitStun.GroundBounce 0
                call "KnockLand"
                shake 10<frames> (3, 5)
                pause 3<frames>
                animate a.HitStun.GroundBounce 1 2
                set Gravity 0.85
                set Vel.Y 12
                On Landing { exit }
                animate a.HitStun.GroundBounce 3 12
                Repeat Var.Body.currentApexFrames { frame a.HitStun.GroundBounce 12 }
                frame a.HitStun.GroundBounce 12
                fitAnimate Var.Body.currentFallFrames a.HitStun.GroundBounce 13 21
                Loop { frame }
            }
        }

        def STATE s.HitStun.WallBounce {
            stance Airborne
            category Hitstun
            skipMeta
            next s.HitStun.LandFront Args.Forward

            action {
                On ActionBegin { stop }
                playSound Sfx.KnockDown.Hard
                pose a.HitStun.WallBounce 0
                sub Origin.Y 30
                If(FinalPosition.Y .< 80) { set FinalPosition.Y 80 }
                effect Vfx.WallHit (ClosestWall 80) [ Rotate(Axis.Y, 75.<deg>) ]
                frame
                shake 20<frames> (4, 6)
                pause 6<frames>
                animate a.HitStun.WallBounce 0 2
                set Gravity p.SlowGravity
                set Vel (7, 14)
                On Landing { exit }
                fitAnimate Var.Body.currentApexFrames a.HitStun.WallBounce 3 30
                animate a.HitStun.WallBounce 31 62
                Loop { frame }
            }
        }

        def STATE s.HitStun.WallSplat {
            stance Airborne
            category Hitstun
            skipMeta
            next s.HitStun.LandFront Args.Forward
            condition (var Position.Y .>= 20)

            action {
                On ActionBegin { stop }
                playSound Sfx.KnockDown.Soft
                pose a.HitStun.WallBounce 0
                sub Origin.Y 30
                If(FinalPosition.Y .< 80) { set FinalPosition.Y 80 }
                effect Vfx.WallHit (ClosestWall 80) [ Rotate(Axis.Y, 75.<deg>) ]
                frame

                shake 20<frames> 3
                pause 6<frames>
                animate a.HitStun.WallBounce 1 10
                set Gravity 0.25

                On Landing {
                    clearEvent
                    gotoParent "landed"
                }

                fitAnimate Var.Body.currentFallFrames a.HitStun.WallBounce 10 12
                Loop { frame }

                label "landed"
                clear Origin.Y
                clear Position.Y
                fitAnimate (num 30) a.HitStun.Crumple 19 56
            }
        }

        def STATE s.HitStun.Air.Flipout {
            stance Airborne
            category Hitstun
            require Airborne
            meta Meta.HitStun.Special
            updateDuringFrameStop
            condition (Args.HitStun.flagged HitFlags.Flipout)
            next s.Jump.Landing

            action {
                On ActionBegin {
                    set Gravity p.Gravity
                    update Position.Y (Max(num 10))
                    set Vel (-3, 12)
                }

                On ActionEnd {
                    clear Gravity
                    clear Vel
                }

                If Var.Entity.isDead { changeState s.HitStun.Air.Die }

                fitAnimate Var.stopFrames a.HitStun.Air.Flipout 1 5
                animate a.HitStun.Air.Flipout 6 25
                On Landing { exit }
                animate a.HitStun.Air.Flipout 26 35
                Loop { frame a.HitStun.Air.Flipout 36 }
            }
        }

        def STATE s.HitStun.Air.SpikedDown {
            stance Airborne
            category Hitstun
            require Airborne
            updateDuringFrameStop
            meta Meta.HitStun.Special
            condition (Args.HitStun.flagged HitFlags.SpikeDown)
            next s.HitStun.LandBack Args.Forward

            let roll = var ()

            action {
                On ActionBegin {
                    set Gravity (p.Gravity * 2.)
                    set Vel (-8, -20)
                    clear roll
                }

                On EachPose { rotateBone bones.Hips roll }

                On EachFrame { set roll (Calc.Remap(Var.currentFrame, num 1, num 6, num 0, num -45)) }

                On ActionEnd {
                    clear Gravity
                    clear Vel
                }

                fitAnimate Var.stopFrames a.HitStun.KnockBack 1 6

                On Landing { exit }

                animate a.HitStun.KnockBack 7 24
                Loop { frame a.HitStun.KnockBack 25 }
            }
        }

        def STATE s.HitStun.Air.Die {
            stance Airborne
            category Hitstun
            require Airborne
            next s.HitStun.LandBack
            skipMeta

            action {
                On ActionBegin {
                    stop
                    disable HurtBox
                    update Position.Y (Max(num 10))
                    set Gravity p.FallGravity
                    set Vel (-3, 10)
                }

                On Landing {
                    stop
                    exit
                }

                fitAnimate Var.Body.currentApexFrames a.HitStun.KnockBack 1 24
                animate a.HitStun.KnockBack 25 44
                Loop { frame }
            }
        }

        def STATE s.St.BlockStun {
            stance Stand
            category Blockstun
            updateDuringFrameStop

            trigger {
                command B
                lenience 0<frames>
                skipRelease
            }

            transitions { empty 3<frames> s.PushBlock }

            let total = Var.Entity.stunFrames + Var.stopFrames

            let deg =
                switch (
                    Args.BlockStun.intensity,
                    [
                        Intensity.Light => -12
                        Intensity.Medium => -20
                        Intensity.Heavy => -30
                        Intensity.Unique => -30
                    ],
                    defaultValue = -10
                )

            action { animateBend total a.St.Guard 33 bones.Spine deg }
        }

        def STATE s.Cr.BlockStun {
            stance Crouch
            category Blockstun
            updateDuringFrameStop

            trigger {
                command DB
                lenience 0<frames>
                skipRelease
            }

            transitions { empty 3<frames> s.PushBlock }

            let deg = var ()
            let total = var ()

            action {
                set total (Var.Entity.stunFrames + Var.stopFrames)

                set
                    deg
                    (switch (
                        Args.BlockStun.intensity,
                        [
                            Intensity.Light => -12
                            Intensity.Medium => -20
                            Intensity.Heavy => -30
                            Intensity.Unique => -30
                        ],
                        defaultValue = -10
                    ))

                animateBend total a.Cr.Guard 33 bones.Spine deg
            }
        }
    }
