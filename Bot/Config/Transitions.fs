namespace PrototypeFighter.Scripts.Bot

open FGScript
open s

module t =
    let standNormals =
        transitions {
            St.LP
            St.MP
            St.HP
            St.LK
            St.MK
            St.HK
            St.F_MP
            St.B_MK
        }

    let crouchNormals =
        transitions {
            Cr.LP
            Cr.MP
            Cr.HP
            Cr.LK
            Cr.MK
            Cr.HK
        }

    let allNormals =
        transitions {
            standNormals
            crouchNormals
        }

    let airNormals =
        transitions {
            Jump.LP
            Jump.MP
            Jump.HP
            Jump.LK
            Jump.MK
            Jump.HK
        }

    let allSpecials =
        transitions {
            Sp.Fireball.L
            Sp.Fireball.M
            Sp.Fireball.H
            Sp.Fireball.EX
            Sp.Tatsu.L
            Sp.Tatsu.M
            Sp.Tatsu.H
            Sp.Tatsu.EX
            Sp.Uppercut.L
            Sp.Uppercut.M
            Sp.Uppercut.H
            Sp.Uppercut.EX
            Sp.DonkeyKick.L
            Sp.DonkeyKick.M
            Sp.DonkeyKick.H
            Sp.DonkeyKick.EX
        }

    let airborneSpecials =
        transitions {
            Sp.DiveKick.L
            Sp.DiveKick.M
            Sp.DiveKick.H
            Sp.DiveKick.EX
        }

    let allJumps =
        transitions {
            Jump.Neutral
            Jump.Forward
            Jump.Backward
        }

    let allDashes =
        transitions {
            Dash.F
            Dash.B
        }

    let regularThrows =
        transitions {
            RegularThrow.Forward
            RegularThrow.Backward
        }

    let allTaunts = transitions { Taunt.Hurry }

    let allAttacks =
        transitions {
            allNormals
            allSpecials
        }

    let specialDashCancellable =
        chainTo Dash.F {
            cancelWindow 22<frames>
            before 30<frames>
            require Stand
            cost r.Stamina 200
            call "StaminaCancelBlink"
        }
