module PrototypeFighter.Scripts.Bot.Specials

open FGScript
open PrototypeFighter.Scripts
open PrototypeFighter.Scripts.Macros

let states =
    def PARTIAL {
        def STATE s.Sp.Fireball.L {
            stance Stand
            category Special
            condition (!Query.Entity.OfType.Projectile())

            trigger {
                command QCF LP
                disallow U
            }

            attack {
                projectile
                light
            }

            transitions { t.specialDashCancellable }

            action {
                phase Startup
                playVoice v.Sp.Fireball (Pitch 2<percent>)
                animate a.Sp.Fireball 4 16
                frame a.Sp.Fireball 18
                frame a.Sp.Fireball 20
                frame a.Sp.Fireball 25
                playSound Sfx.Fireball
                effect Vfx.Fireball.Flash At.Hand
                spawn "Fireball" At.Hint (args 6)
                phase Recovery
                animate a.Sp.Fireball 33 66
            }
        }

        def STATE s.Sp.Fireball.M {
            stance Stand
            category Special
            condition (!Query.Entity.OfType.Projectile())

            trigger { command QCF MP }
            transitions { t.specialDashCancellable }

            attack {
                projectile
                light
            }

            action {
                phase Startup
                playVoice v.Sp.Fireball (Pitch 2<percent>)
                animate a.Sp.Fireball 6 15
                frame a.Sp.Fireball 18
                frame a.Sp.Fireball 20
                frame a.Sp.Fireball 25
                playSound Sfx.Fireball
                effect Vfx.Fireball.Flash At.Hand
                spawn "Fireball" At.Hint (args 8)

                phase Recovery
                frame a.Sp.Fireball 26
                frame a.Sp.Fireball 27
                animate a.Sp.Fireball 33 66
            }
        }

        def STATE s.Sp.Fireball.H {
            stance Stand
            category Special
            condition (!Query.Entity.OfType.Projectile())

            trigger { command QCF HP }

            transitions { t.specialDashCancellable }

            attack {
                projectile
                medium
            }

            action {
                phase Startup
                playVoice v.Sp.Fireball (Pitch 2<percent>)
                animate a.Sp.Fireball 6 15

                On EachPose { transformBone bones.Spine2 (Rotate(Axis.X, -20.0<deg>)) }

                frame a.Sp.Fireball 20
                frame a.Sp.Fireball 25
                playSound Sfx.Fireball
                effect Vfx.Fireball.Flash At.Hand [ Translate(Axis.X, -5); Translate(Axis.Y, 20) ]
                spawn "Fireball" (Around(At.Foot, 150, 60<deg>)) 25<deg> (args 8)

                phase Recovery
                frame a.Sp.Fireball 26
                frame a.Sp.Fireball 27
                animate a.Sp.Fireball 33 66
            }
        }

        def STATE s.Sp.Fireball.EX {
            stance Stand
            category Special
            priority 1
            cost r.Energy 100
            condition (!Query.Entity.OfType.Projectile())

            trigger { command QCF PP }

            transitions { t.specialDashCancellable }

            attack {
                projectile
                heavy
            }

            action {
                On ActionBegin {
                    aura Vfx.Auras.EX
                    playSound Sfx.ActivateEX
                }

                phase Startup
                playVoice v.Sp.Fireball (Pitch 2<percent>)
                animate a.Sp.Fireball 6 15
                frame a.Sp.Fireball 18
                frame a.Sp.Fireball 20
                frame a.Sp.Fireball 25
                playSound Sfx.Fireball
                effect Vfx.Fireball.Flash At.Hand
                spawn "FireballEX" At.Hint (args 10)

                phase Recovery
                clearAura Vfx.Auras.EX
                frame a.Sp.Fireball 26
                frame a.Sp.Fireball 27
                animate a.Sp.Fireball 33 66
            }
        }

        def STATE s.Sp.Tatsu.L {
            stance Stand
            category Special

            trigger { command HCB LK }

            attack {
                strike
                light
                kick
                looks High
                hitSound Sfx.Hit.Heavy
                force Stand
                damage 80
                gain r.Energy 45
                set Stun.MaxLevel
                set JuggleLimit p.JL.Specials
                set KnockBack OnHit (5.0, 8.5) p.FallGravity
                set KnockUp (On(CounterHit, Punish)) (1, 12) p.SlowGravity
                pushback (On(CounterHit, Punish)) 10
                armorBreak
                data 1
            }

            action {
                On ActionBegin {
                    set Vel.X 6
                    set Gravity 0
                }

                On ActionEnd {
                    set Vel.X 0
                    set Offset.Y 0
                }

                On DidHit { shake 20<frames> (4, 6) }

                phase Startup
                playVoice v.Sp.Tatsu (Pitch 2<percent>)
                animate a.Sp.Tatsu.Start 1 4
                frame a.Sp.Tatsu.Start 6
                frame a.Sp.Tatsu.Start 7
                frame a.Sp.Tatsu.Start 9
                frame a.Sp.Tatsu.Start 10
                effect Vfx.Dust At.Foot
                stance Airborne
                move Upward 10
                frame a.Sp.Tatsu.Start 12
                frame a.Sp.Tatsu.Start 13
                move Upward 10
                frame a.Sp.Tatsu.Start 15
                frame a.Sp.Tatsu.Start 16
                move Upward 10

                phase Active
                playSound Sfx.Swing.HK
                animate a.Sp.Tatsu.Loop 0 2

                phase Recovery
                set Friction 0.3
                move Downward 5
                animate a.Sp.Tatsu.Loop 3 6
                move Downward 5
                animate a.Sp.Tatsu.End 3 10
                move Downward 10
                animate a.Sp.Tatsu.End 11 15
                move Downward 10
                stance Default
                animate a.Sp.Tatsu.End 16 31
            }
        }

        def STATE s.Sp.Tatsu.M {
            stance Stand
            category Special

            trigger { command HCB MK }

            attack {
                strike
                medium
                kick
                looks High
                hitSound Sfx.Hit.Heavy
                force Stand
                damage 50
                gain r.Energy 25
                set Stun.MaxLevel
                set JuggleStart 4
                set JuggleIncrease 2
                set JuggleLimit p.JL.Specials
                pushbackFriction 100<percent>
                pushback OnHit 80
                set Air KnockBack (5, 5) p.FallGravity
            }

            action {
                On ActionBegin {
                    set Vel.X 6
                    set Gravity 0
                    disable ClashBox
                }

                On ActionEnd {
                    set Vel.X 0
                    set Offset.Y 0
                }

                On DidHit { shake 20<frames> (8, 10) }

                phase Startup
                playVoice v.Sp.Tatsu (Pitch 2<percent>)
                animate a.Sp.Tatsu.Start 1 4
                animate a.Sp.Tatsu.Start 6 10
                effect Vfx.Dust At.Foot
                stance Airborne
                move Upward 10
                animate a.Sp.Tatsu.Start 12 13
                move Upward 10
                animate a.Sp.Tatsu.Start 14 16
                move Upward 10

                phase Active
                playSound Sfx.Swing.HK
                animate a.Sp.Tatsu.Loop 0 5
                animate a.Sp.Tatsu.Loop 9 15
                animate a.Sp.Tatsu.Loop 19 23

                resetAttackKnock Air
                attack KnockBack (5.0, 8.5) p.FallGravity
                hitAgain
                add JuggleLimit 2
                playSound Sfx.Swing.HK
                pushback OnHit 10
                animate a.Sp.Tatsu.Loop 0 2

                phase Recovery
                set Friction 0.3
                move Downward 5
                animate a.Sp.Tatsu.Loop 3 6
                move Downward 5
                animate a.Sp.Tatsu.End 3 10
                move Downward 10
                animate a.Sp.Tatsu.End 11 15
                move Downward 10
                stance Default
                animate a.Sp.Tatsu.End 16 31
            }
        }

        def STATE s.Sp.Tatsu.H {
            stance Stand
            category Special

            trigger { command HCB HK }

            attack {
                strike
                heavy
                kick
                looks High
                hitSound Sfx.Hit.Heavy
                force Stand
                damage 40
                gain r.Energy 25
                set Stun.MaxLevel
                set JuggleStart 4
                set JuggleIncrease 2
                set JuggleLimit p.JL.Specials
                pushback OnHit 80
                pushbackFriction 100<percent>
                set Air KnockBack (5, 5) p.FallGravity
            }

            action {
                On ActionBegin {
                    set Vel.X 6
                    set Gravity 0
                    disable ClashBox
                }

                On ActionEnd {
                    set Vel.X 0
                    set Offset.Y 0
                }

                On DidHit { shake 20<frames> (8, 10) }

                phase Startup
                playVoice v.Sp.Tatsu (Pitch 2<percent>)
                animate a.Sp.Tatsu.Start 1 10
                effect Vfx.Dust At.Foot
                stance Airborne
                move Upward 10
                animate a.Sp.Tatsu.Start 11 13
                move Upward 10
                animate a.Sp.Tatsu.Start 14 16
                move Upward 10

                phase Active
                playSound Sfx.Swing.HK
                animate a.Sp.Tatsu.Loop 0 5
                animate a.Sp.Tatsu.Loop 9 15
                animate a.Sp.Tatsu.Loop 19 23

                resetAttackKnock Air
                attack KnockBack (1, 8) p.FallGravity
                hitAgain
                add JuggleLimit JuggleIncrease
                attackData 2
                playSound Sfx.Swing.HK
                animate a.Sp.Tatsu.Loop 0 5
                animate a.Sp.Tatsu.Loop 9 15
                animate a.Sp.Tatsu.Loop 19 23

                hitAgain
                add JuggleLimit JuggleIncrease
                attack KnockUp (5.0, 11.5) p.KnockGravity
                pushback OnHit 30
                playSound Sfx.Swing.HK
                animate a.Sp.Tatsu.Loop 0 2

                phase Recovery
                set Friction 0.3
                move Downward 5
                animate a.Sp.Tatsu.Loop 3 6
                move Downward 5
                animate a.Sp.Tatsu.End 3 10
                move Downward 10
                animate a.Sp.Tatsu.End 11 15
                move Downward 10
                stance Default
                animate a.Sp.Tatsu.End 16 31
            }
        }

        def STATE s.Sp.Tatsu.EX {
            stance Stand
            category Special
            priority 1
            cost r.Energy 100

            trigger { command HCB KK }

            let pushAmount = 170

            attack {
                strike
                heavy
                kick
                looks High
                hitSound Sfx.Hit.Heavy
                force Stand
                damage 30
                gain r.Energy 7.5
                set BlockStun 16<frames>
                set HitStop 8<frames>
                set HitStun 12<frames>
                set JuggleLimit p.JL.ExSpecials
                set KnockUp (0.0, 2.5) p.KnockGravity
                pushback OnBlock 60
                pushbackFriction 100<percent>
            }

            action {
                On ActionBegin {
                    clear Vel.X
                    clear Gravity
                    enable Intangible
                    disable ClashBox
                    enable BoxLayer.Layer2
                    aura Vfx.Auras.EX
                    playSound Sfx.ActivateEX
                }

                On ActionEnd {
                    set Offset.Y 0
                    disable Intangible
                }

                phase Startup
                playVoice v.Sp.Tatsu (Pitch 2<percent>)
                animate a.Sp.Tatsu.Start 1 4
                animate a.Sp.Tatsu.Start 6 10
                effect Vfx.Dust Floor [ Translate(Axis.X, -50); Scale(Axis.XYZ, 0.8); Rotate(Axis.Z, -15.<deg>) ]
                effect Vfx.Dust Floor [ Translate(Axis.X, 50); Scale(Axis.XYZ, 0.8); Rotate(Axis.Z, -165.<deg>) ]
                stance Airborne
                move Upward 10
                animate a.Sp.Tatsu.Start 12 13
                move Upward 10
                animate a.Sp.Tatsu.Start 15 16
                move Upward 10

                phase Active

                Repeat 2<times> {
                    vacuum OnHit pushAmount
                    playSound Sfx.Swing.HK
                    animate a.Sp.Tatsu.Loop 0 2
                    frame a.Sp.Tatsu.Loop 3
                    frame a.Sp.Tatsu.Loop 5
                    animate a.Sp.Tatsu.Loop 9 10
                    hitAgain
                    inc JuggleLimit
                    pushback OnHit pushAmount
                    animate a.Sp.Tatsu.Loop 13 15
                    frame a.Sp.Tatsu.Loop 16
                    frame a.Sp.Tatsu.Loop 17
                    frame a.Sp.Tatsu.Loop 19
                    frame a.Sp.Tatsu.Loop 23
                    effect Vfx.Dust Floor [ Translate(Axis.X, -50); Scale(Axis.XYZ, 0.8); Rotate(Axis.Z, -15.<deg>) ]
                    effect Vfx.Dust Floor [ Translate(Axis.X, 50); Scale(Axis.XYZ, 0.8); Rotate(Axis.Z, -165.<deg>) ]
                    hitAgain
                    inc JuggleLimit
                }

                attack KnockUp (5.0, 12.5) p.KnockGravity
                pushback OnHit 30
                playSound Sfx.Swing.HK
                animate a.Sp.Tatsu.Loop 0 2

                phase Recovery
                clearAura Vfx.Auras.EX
                move Downward 5
                animate a.Sp.Tatsu.Loop 3 6
                move Downward 5
                animate a.Sp.Tatsu.End 3 10
                move Downward 10
                animate a.Sp.Tatsu.End 11 15
                move Downward 10
                stance Default
                animate a.Sp.Tatsu.End 16 31
            }
        }

        let UppercutGravity = 0.82

        def STATE s.Sp.Uppercut.L {
            stance Stand
            category Special

            trigger { command DP LP }

            attack {
                light
                punch
                damage 85
                gain r.Energy 40
                hitSound Sfx.Hit.Heavy
                set Stun.Level[4]
                set KnockUp (3, 10) p.FallGravity
                set JuggleLimit (p.JL.Specials * 2)
                set Air KnockBack OnHit (3, 9) p.FallGravity
                set Air KnockUp (On(CounterHit, Punish)) (0.6, 13.0) p.KnockGravity
            }

            action {
                On ActionEnd {
                    clear Vel
                    clear Acc
                }

                phase Startup
                playVoice v.Sp.Uppercut (Pitch 2<percent>)
                animate a.Sp.Uppercut 0 1
                frame a.Sp.Uppercut 3
                frame a.Sp.Uppercut 5

                phase Active
                set Vel.X 2.5
                playSound Sfx.Swing.MP
                animate a.Sp.Uppercut 6 8

                stance Airborne
                set Gravity UppercutGravity
                set Vel.Y 11
                effect Vfx.Dust [ Rotate(Axis.Z, -105.<deg>); Scale(Axis.XYZ, 1.5) ]
                animate a.Sp.Uppercut 9 14
                set Friction 0.3
                frame a.Sp.Uppercut 15

                phase Recovery
                Repeat(Var.Body.currentApexFrames - 1) { frame a.Sp.Uppercut 19 }
                animate a.Sp.Uppercut 20 25
                clear Vel.X
                clear Friction
                On Landing { gotoParent "landing" }
                animate a.Sp.Uppercut 26 50

                label "landing"
                stance Default
                call "LandEffect"
                animate a.Sp.Uppercut 51 62
            }
        }

        def STATE s.Sp.Uppercut.M {
            stance Stand
            category Special

            trigger { command DP MP }

            attack {
                medium
                punch
                damage 100
                gain r.Energy 40
                hitSound Sfx.Hit.Heavy
                set Stun.Level[5]
                set JuggleLimit p.JL.Specials
                set KnockUp (3, 12) p.FallGravity
                set Air KnockBack (3, 11) p.FallGravity
            }

            action {
                On ActionEnd {
                    clear Vel
                    clear Acc
                }

                phase Startup
                playVoice v.Sp.Uppercut (Pitch 2<percent>)
                animate a.Sp.Uppercut 0 3
                frame a.Sp.Uppercut 5

                phase Active
                set Vel.X 4.2
                playSound Sfx.Swing.HP
                animate a.Sp.Uppercut 6 8

                stance Airborne
                set Gravity UppercutGravity
                set Vel.Y 12.5
                effect Vfx.Dust [ Rotate(Axis.Z, -105.<deg>); Scale(Axis.XYZ, 1.5) ]
                animate a.Sp.Uppercut 9 14
                set Friction 0.3
                frame a.Sp.Uppercut 15

                phase Recovery
                Repeat(Var.Body.currentApexFrames - 1) { frame a.Sp.Uppercut 19 }
                animate a.Sp.Uppercut 20 25
                clear Vel.X
                clear Friction
                On Landing { gotoParent "landing" }
                animate a.Sp.Uppercut 26 50

                label "landing"
                stance Default
                call "LandEffect"
                animate a.Sp.Uppercut 51 62
            }
        }

        def STATE s.Sp.Uppercut.H {
            stance Stand
            category Special

            trigger { command DP HP }

            transitions { t.specialDashCancellable }

            attack {
                heavy
                punch
                damage 120
                gain r.Energy 40
                hitSound Sfx.Hit.Heavy
                set Stun.Level[6]
                set JuggleLimit p.JL.Specials
                set KnockUp (3.6, 14.0) p.FallGravity
                set Air KnockBack (3.6, 13.0) p.FallGravity
                extra HitStop 4<frames>
            }

            action {
                On ActionEnd {
                    clear Vel
                    clear Acc
                }

                phase Startup
                playVoice v.Sp.Uppercut (Pitch 2<percent>)
                animate a.Sp.Uppercut 0 3
                set Vel.X 10
                animate a.Sp.Uppercut 4 5

                phase Active
                set Vel.X 5
                playSound Sfx.Swing.HP
                animate a.Sp.Uppercut 6 8

                stance Airborne
                set Gravity UppercutGravity
                set Vel.Y 16
                effect Vfx.Dust [ Rotate(Axis.Z, -105.<deg>); Scale(Axis.XYZ, 1.5) ]
                animate a.Sp.Uppercut 9 14
                set Friction 0.3
                frame a.Sp.Uppercut 15

                phase Recovery
                Repeat(Var.Body.currentApexFrames - 1) { frame a.Sp.Uppercut 19 }
                animate a.Sp.Uppercut 20 25
                clear Vel.X
                clear Friction
                On Landing { gotoParent "landing" }
                animate a.Sp.Uppercut 26 50

                label "landing"
                stance Default
                call "LandEffect"
                animate a.Sp.Uppercut 51 62
            }
        }

        def STATE s.Sp.Uppercut.EX {
            stance Stand
            category Special
            priority 1
            cost r.Energy 100

            trigger { command DP PP }

            transitions {
                chainTo s.Sp.DiveKick.EX {
                    cancelWindow 45<frames>
                    lenience 12<frames>
                    after 20<frames>
                    before 48<frames>
                    skip 5<frames>
                    cost r.Stamina 200
                    call "SpecialCancelBlink"
                }
            }

            attack {
                heavy
                punch
                damage 70
                gain r.Energy 20
                hitSound Sfx.Hit.Heavy
                set Stun.Level[6]
                set KnockUp (3.5, 14.0) p.FallGravity
                set Air KnockBack (3.5, 14.0) p.FallGravity
                extra HitStop 6<frames>
            }

            action {
                On ActionBegin {
                    aura Vfx.Auras.EX
                    playSound Sfx.ActivateEX
                    enable FullyInvuln
                }

                On ActionEnd {
                    clear Vel
                    clear Acc
                    disable FullyInvuln
                }

                phase Startup
                playVoice v.Sp.Uppercut (Pitch 2<percent>)
                animate a.Sp.Uppercut 0 1
                set Vel.X 10
                frame a.Sp.Uppercut 3
                frame a.Sp.Uppercut 5

                phase Active
                set Vel.X 5.5
                playSound Sfx.Swing.HP
                animate a.Sp.Uppercut 6 8

                stance Airborne
                set Gravity UppercutGravity
                set Vel.Y 16
                effect Vfx.Dust [ Rotate(Axis.Z, -105.<deg>); Scale(Axis.XYZ, 1.5) ]
                animate a.Sp.Uppercut 9 10
                hitAgain
                animate a.Sp.Uppercut 11 14
                set Friction 0.3
                frame a.Sp.Uppercut 15

                phase Recovery
                Repeat(Var.Body.currentApexFrames - 1) { frame a.Sp.Uppercut 19 }
                animate a.Sp.Uppercut 20 25
                clear Vel.X
                clear Friction
                enable FullyInvuln
                On Landing { gotoParent "landing" }
                animate a.Sp.Uppercut 26 50

                label "landing"
                stance Default
                clearAura Vfx.Auras.EX
                call "LandEffect"
                animate a.Sp.Uppercut 51 62
            }
        }

        def STATE s.Sp.DonkeyKick.L {
            stance Stand
            category Special
            require Grounded

            trigger { command !B F LK }
            transitions { t.specialDashCancellable }

            attack {
                light
                kick
                hitSound Sfx.Hit.Heavy
                damage 100
                damage r.Stamina 15 OnBlock
                gain r.Energy 30
                set Stun.MaxLevel
                set JuggleLimit p.JL.Specials
                set KnockBack (5.0, 8.5) p.FallGravity
            }

            action {
                On ActionEnd {
                    clear Vel.X
                    clear Acc.X
                }

                On DidHit {
                    set Friction 2
                    shake 12<frames> (6, 8)
                }

                phase Startup
                playVoice v.Attack.Heavy
                animate a.Sp.DonkeyKick 14 24
                playSound Sfx.Swing.HK
                animate a.Sp.DonkeyKick 25 26

                phase Active
                effect Vfx.Dust At.Foot [ (Scale(Axis.XY, 1.25)) ]
                set Vel.X 18
                set Friction 1
                animate a.Sp.DonkeyKick 27 32

                phase Recovery
                disable HitBox ProximityBox
                clear Vel.X
                animate a.Sp.DonkeyKick 34 55
                frame a.Sp.DonkeyKick 65
            }
        }

        def STATE s.Sp.DonkeyKick.M {
            stance Stand
            category Special
            require Grounded

            trigger { command !B F MK }
            transitions { t.specialDashCancellable }

            attack {
                medium
                kick
                hitSound Sfx.Hit.Heavy
                damage 110
                damage r.Stamina 20 OnBlock
                gain r.Energy 40
                set Stun.MaxLevel
                set JuggleLimit p.JL.Specials
                set BlockStun 15<frames>
                set KnockBack (6, 9) p.FallGravity
            }

            action {
                On ActionEnd {
                    clear Vel.X
                    clear Acc.X
                }

                On DidHit {
                    set Friction 2
                    shake 20<frames> (6, 8)
                }

                phase Startup
                playVoice v.Attack.Heavy
                animate a.Sp.DonkeyKick 10 20
                playSound Sfx.Swing.HK
                animate a.Sp.DonkeyKick 21 26

                phase Active
                effect Vfx.Dust At.Foot [ (Scale(Axis.XY, 1.25)) ]
                set Vel.X 20
                set Friction 1.2
                animate a.Sp.DonkeyKick 27 36

                phase Recovery
                clear Vel.X
                animate a.Sp.DonkeyKick 38 56
                animate a.Sp.DonkeyKick 62 65
            }
        }

        def STATE s.Sp.DonkeyKick.H {
            stance Stand
            category Special
            require Grounded

            trigger { command !B F HK }

            transitions { t.specialDashCancellable }

            attack {
                heavy
                kick
                hitSound Sfx.Hit.Heavy
                damage 130
                damage r.Stamina 30 OnBlock
                gain r.Energy 45
                set Stun.MaxLevel
                set JuggleLimit p.JL.Specials
                set BlockStun 16<frames>
                set KnockBack (7, 10) p.FallGravity
                set HitFlags.WallSplat

                set KnockBack (On(Punish)) (25, 9) p.SlowGravity
                set (On(Punish)) HitFlags.WallBounce
            }

            action {
                On ActionEnd {
                    clear Vel.X
                    clear Acc.X
                }

                On DidHit {
                    set Friction 2.4
                    shake 20<frames> (6, 8)
                }

                phase Startup
                animate a.Sp.DonkeyKick 0 5
                playVoice v.Attack.Heavy
                animate a.Sp.DonkeyKick 6 20
                playSound Sfx.Swing.HK
                animate a.Sp.DonkeyKick 21 26

                phase Active
                effect Vfx.Dust At.Foot [ (Scale(Axis.XY, 1.25)) ]
                set Vel.X 25
                set Friction 1.2
                animate a.Sp.DonkeyKick 27 30
                animate a.Sp.DonkeyKick 31 36

                phase Recovery
                clear Vel.X
                animate a.Sp.DonkeyKick 38 55
                animate a.Sp.DonkeyKick 60 65
            }
        }

        def STATE s.Sp.DonkeyKick.EX {
            stance Stand
            category Special
            require Grounded
            priority 1
            cost r.Energy 100
            trigger { command !B F KK }

            attack {
                heavy
                kick
                hitSound Sfx.Hit.Heavy
                damage 80
                damage r.Stamina 10 OnBlock
                gain r.Energy 10
                set Stun.MaxLevel
                set JuggleLimit p.JL.ExSpecials
                set BlockStun 12<frames>
                set KnockBack (26.0, 9.5) p.SlowGravity
                armorBreak
                set HitFlags.WallBounce
            }

            action {
                On ActionBegin {
                    superArmor
                    aura Vfx.Auras.EX
                    playSound Sfx.ActivateEX
                    enable BoxLayer.Layer2
                }

                On ActionEnd {
                    clear Vel.X
                    clear Acc.X
                }

                On DidHit {
                    set Friction 2.2
                    shake 20<frames> (6, 8)
                }

                phase Startup
                animate a.Sp.DonkeyKick 5 10
                playVoice v.Attack.Heavy
                animate a.Sp.DonkeyKick 11 21
                playSound Sfx.Swing.HK
                animate a.Sp.DonkeyKick 22 26

                phase Active
                effect Vfx.Dust At.Foot [ (Scale(Axis.XY, 1.25)) ]
                set Vel.X 24
                set Friction 0.8
                frame a.Sp.DonkeyKick 27
                frame a.Sp.DonkeyKick 28
                frame a.Sp.DonkeyKick 30
                frame a.Sp.DonkeyKick 32
                frame a.Sp.DonkeyKick 34
                frame a.Sp.DonkeyKick 35

                phase Recovery
                disable HitBox ProximityBox
                clear Vel.X
                frame a.Sp.DonkeyKick 36 2<times>
                frame a.Sp.DonkeyKick 37 2<times>
                clearAura Vfx.Auras.EX
                animate a.Sp.DonkeyKick 38 59
                animate a.Sp.DonkeyKick 60 65
            }
        }

        def STATE s.Sp.DiveKick.L {
            stance Airborne
            category Special

            require Airborne
            condition (var Position.Y .>= 50)

            trigger {
                command QCB LK
                motionLenience 16<frames>
            }

            attack {
                light
                kick
                hitSound Sfx.Hit.Medium
                damage 60
                gain r.Energy 20
                set HitStun 24<frames>
                set BlockStun 15<frames>
                set HitStop 13<frames>
                set JuggleLimit p.JL.Specials
                set HitFlags.SpikeDown
            }

            defaultArgs (args 5)

            action {
                On ActionBegin {
                    stop
                    force Air KnockUp (3, 8) p.KnockGravity
                }

                phase Startup
                frame a.Sp.DiveKick 0
                playVoice v.Attack.Medium
                animate a.Sp.DiveKick 1 15

                phase Active
                On Landing { gotoParent "landed" }
                On ActionEnd { clear Vel }
                playSound Sfx.Swing.MK
                set Vel.X Arg1
                set Vel.Y -14
                frame a.Sp.DiveKick 16

                Repeat(Var.Body.currentFallFrames - 3) { frame }
                attackExtra HitStun 3<frames>
                On(DidHit, GotBlocked) { shake 14<frames> 6.5 }
                Loop { frame }

                label "landed"
                clearEvent Landing
                stance Stand
                clear Vel

                phase Recovery
                frame a.Sp.DiveKickLanding 0
                playSound Sfx.Landing
                call "LandEffect"
                fitAnimate (num 17) a.Sp.DiveKickLanding 1 23
            }
        }

        def STATE s.Sp.DiveKick.M {
            stance Airborne
            category Special

            require Airborne
            condition (var Position.Y .>= 50)

            trigger {
                command QCB MK
                motionLenience 16<frames>
            }

            attack {
                medium
                kick
                hitSound Sfx.Hit.Medium
                damage 60
                gain r.Energy 20
                set HitStun 24<frames>
                set BlockStun 15<frames>
                set HitStop 13<frames>
                set JuggleLimit p.JL.Specials
                set HitFlags.SpikeDown
            }

            defaultArgs (args 10)
            useAction s.Sp.DiveKick.L
        }

        def STATE s.Sp.DiveKick.H {
            stance Airborne
            category Special

            require Airborne
            condition (var Position.Y .>= 50)

            trigger {
                command QCB HK
                motionLenience 16<frames>
            }

            transitions { t.specialDashCancellable }

            attack {
                heavy
                kick
                hitSound Sfx.Hit.Medium
                damage 60
                gain r.Energy 20
                set HitStun 24<frames>
                set BlockStun 15<frames>
                set HitStop 13<frames>
                set JuggleLimit p.JL.Specials
                set HitFlags.SpikeDown
            }

            defaultArgs (args 15)
            useAction s.Sp.DiveKick.L
        }

        def STATE s.Sp.DiveKick.EX {
            stance Airborne
            category Special

            require Airborne
            condition (var Position.Y .>= 50)
            cost r.Energy 100

            trigger {
                command QCB KK
                motionLenience 16<frames>
            }

            attack {
                unique
                kick
                hitSound Sfx.Hit.Heavy
                damage 80
                gain r.Energy 10
                set HitStun 26<frames>
                set BlockStun 18<frames>
                set HitStop 13<frames>
                set JuggleLimit p.JL.ExSpecials
                set HitFlags.SpikeDown HitFlags.GroundBounce
            }

            action {
                On ActionBegin {
                    stop
                    force Air KnockUp (3, 8) p.KnockGravity
                    aura Vfx.Auras.EX
                    playSound Sfx.ActivateEX
                }

                phase Startup
                frame a.Sp.DiveKick 0
                playVoice v.Attack.Medium
                animate a.Sp.DiveKick 3 15

                phase Active
                On Landing { gotoParent "landed" }
                On ActionEnd { clear Vel }
                playSound Sfx.Swing.MK
                set Vel (12, -14)
                frame a.Sp.DiveKick 16

                Repeat(Var.Body.currentFallFrames - 3) { frame }
                attackExtra HitStun 4<frames>
                On(DidHit, GotBlocked) { shake 14<frames> 6.5 }
                Loop { frame }

                label "landed"
                clearEvent Landing
                stance Stand
                clear Vel

                phase Recovery
                frame a.Sp.DiveKickLanding 0
                clearAura Vfx.Auras.EX
                playSound Sfx.Landing
                call "LandEffect"
                fitAnimate (num 11) a.Sp.DiveKickLanding 1 23
            }
        }
    }
