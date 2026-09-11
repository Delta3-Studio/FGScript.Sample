module PrototypeFighter.Scripts.Bot.Projectiles

open PrototypeFighter.Scripts
open FGScript

let main = [
    def PROJECTILE "Fireball" {
        On INIT {
            log "BEGIN: Projectile - Speed:" (var Arg1)
            set Vel.X Arg1
            turn (var Rotation)
            send Signal.Parent (Gen.Random(0, 10))

            enable BoxLayer.Layer2
            Timer 3<frames> { disable BoxLayer.Layer2 }
        }

        On DESTROYED {
            effect Vfx.Fireball.Hit
            log "END: Projectile"
        }

        On DidContact { destroy }

        On LeaveScreen {
            log "LEAVE: Projectile" Position
            destroy
        }

        On Clash {
            log "CLASH: Projectile" Args.OnClash.otherEntityId
            playSound Sfx.Clash

            If(Args.OnClash.priority .> Args.OnClash.otherPriority) { pass }
            destroy
        }

        On Signal.Parent { log "MSG: Parent " Arg1 Args.Last }

        On(Parent, Signal.Opponent) { log "OWNER: From Opponent" Arg1 Arg2 Args.Last }

        On(Parent, GotHit) {
            log "OWNER: Got-Hit"
            destroy
        }

        On(Parent, NEUTRAL) { log "OWNER: Is Neutral" }

        attack {
            projectile
            damage 70
            damage r.Stamina 18 OnBlock
            gain r.Energy 25
            set Stun.Level[3]
            set HitStopTaken 0<frames>
            set JuggleLimit p.JL.Specials
            set Air KnockBack (3, 9) p.Gravity
            hitEffect OriginPoint Vfx.HitEffect.Fire
        }

        action {
            frame 300<times>
            log "SPOIL: Projectile"
        }
    }

    def PROJECTILE "FireballEX" {
        let hitCount = var ()

        On INIT {
            set Vel.X Arg1
            set hitCount 3

            enable BoxLayer.Layer2
            Timer 3<frames> { disable BoxLayer.Layer2 }
        }

        On DESTROYED { effect Vfx.Fireball.Hit }
        On DidContact { call "NextHit" }

        On LeaveScreen { destroy }

        On Clash {
            playSound Sfx.Clash
            If(Args.OnClash.priority .> Args.OnClash.otherPriority) { pass }
            call "NextHit"
        }

        def FUNC "NextHit" {
            dec hitCount
            If(hitCount .<= Zero) { destroy }

            If(hitCount == One) {
                attack KnockBack (3.0, 9.6) 0.8
                attack Stun.Level[5]
                attack HitStopTaken 0<frames>
            }

            enable EntityFlags.SkipScaling
            hitAgain
        }

        attack {
            projectile
            damage 40
            damage r.Stamina 16 OnBlock
            gain r.Energy 10
            set Stun.Level[3]
            set JuggleLimit p.JL.ExSpecials
            set KnockBack (2, 6) 0.85
            pushback OnHit 30
            pushbackLinear
            hitEffect OriginPoint Vfx.HitEffect.Fire
        }

        action {
            On(DidHit, GotBlocked) { log "Available Hits" hitCount }
            frame 240<times>
        }
    }

]
