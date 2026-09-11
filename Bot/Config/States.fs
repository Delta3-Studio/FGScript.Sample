namespace PrototypeFighter.Scripts.Bot

module s =
    let Neutral = "Neutral"

    let St = {|
        LP = "s.LP"
        MP = "s.MP"
        HP = "s.HP"
        LK = "s.LK"
        MK = "s.MK"
        HK = "s.HK"
        F_MP = "s.F+MP"
        B_MK = "s.B+MK"
        Guard = "s.Guard"
        BlockStun = "s.BlockStun"
    |}

    let Cr = {|
        Neutral = "Crouch"
        LP = "c.LP"
        MP = "c.MP"
        HP = "c.HP"
        LK = "c.LK"
        MK = "c.MK"
        HK = "c.HK"
        Guard = "c.Guard"
        BlockStun = "c.BlockStun"
    |}

    let Jump = {|
        Neutral = "j.Neutral"
        Forward = "j.Forward"
        Backward = "j.Backward"
        Landing = "j.Landing"
        LK = "j.LK"
        MK = "j.MK"
        HK = "j.HK"
        LP = "j.LP"
        MP = "j.MP"
        HP = "j.HP"
    |}

    let Chain = {|
        MP_HP = "MP>HP"
        MP_HP_HK = "MP>HP>HK"
        cMP_cHP = "cMP>cHP"
        jMK_jHK = "jMK>jHK"
    |}

    let HitStun = {|
        Stand = {|
            Legs = "HitStun.St.Legs"
            Body = "HitStun.St.Body"
            Head = "HitStun.St.Head"
        |}
        Air = {|
            Default = "HitStun.Air"
            Flipout = "HitStun.Flipout"
            SpikedDown = "HitStun.SpikedDown"
            Die = "HitStun.Air.Die"
        |}
        Crouch = "HitStun.Cr"
        Crumple = "HitStun.Crumple"
        Stagger = "HitStun.Stagger"
        KnockDown = "HitStun.KnockDown"
        LandBack = "HitStun.LandBack"
        LandFront = "HitStun.LandFront"
        KnockBack = "HitStun.KnockBack"
        KnockUp = "HitStun.KnockUp"
        GroundBounce = "HitStun.GroundBounce"
        WallBounce = "HitStun.WallBounce"
        WallSplat = "HitStun.WallSplat"
    |}

    let Fall = "Air.Fall"

    let RegularThrow = {|
        Forward = "RegularThrow.F"
        ForwardExec = "RegularThrow.F.Exec"
        ForwardStun = "RegularThrow.F.Stun"
        Backward = "RegularThrow.B"
        BackwardExec = "RegularThrow.B.Exec"
        Escape = "RegularThrow.Escape"
        EscapePushed = "RegularThrow.EscapePushed"
    |}

    let LyingDown = {|
        FaceUp = "LyingDown.FaceUp"
        FaceDown = "LyingDown.FaceDown"
    |}

    let WakeUp = {|
        FaceUp = "WakeUp.FaceUp"
        FaceDown = "WakeUp.FaceDown"
        FaceUpQuick = "WakeUp.FaceUp.Quick"
        RollForward = "WakeUp.Roll.F"
        RollBackward = "WakeUp.Roll.B"
    |}

    let Walk = {| F = "Walk.F"; B = "Walk.B" |}
    let Dash = {| F = "Dash.F"; B = "Dash.B" |}
    let Taunt = {| Hurry = "Taunt.Hurry" |}
    let PushBlock = "PushBlock"

    let Anim = {|
        Outro = {|
            DieStand = "Outro.Die.Stand"
            Victory = "Outro.Victory"
            Timeout = "Outro.Timeout"
        |}
    |}

    let Sp = {|
        Fireball = {|
            L = "Sp.Fireball.L"
            M = "Sp.Fireball.M"
            H = "Sp.Fireball.H"
            EX = "Sp.Fireball.EX"
        |}
        Tatsu = {|
            L = "Sp.Tatsu.L"
            M = "Sp.Tatsu.M"
            H = "Sp.Tatsu.H"
            EX = "Sp.Tatsu.EX"
        |}
        Uppercut = {|
            L = "Sp.Uppercut.L"
            M = "Sp.Uppercut.M"
            H = "Sp.Uppercut.H"
            EX = "Sp.Uppercut.EX"
        |}
        DonkeyKick = {|
            L = "Sp.DonkeyKick.L"
            M = "Sp.DonkeyKick.M"
            H = "Sp.DonkeyKick.H"
            EX = "Sp.DonkeyKick.EX"
        |}
        DiveKick = {|
            L = "Sp.DiveKick.L"
            M = "Sp.DiveKick.M"
            H = "Sp.DiveKick.H"
            EX = "Sp.DiveKick.EX"
        |}
    |}
