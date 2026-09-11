namespace PrototypeFighter.Scripts.Bot

module a =
    let Walk = {| F = "walk_f"; B = "walk_b" |}
    let Dash = {| F = "dash_f"; B = "dash_b" |}

    let St = {|
        Idle = "st_idle"
        LP = "st_lp"
        MP = "st_mp"
        HP = "st_hp"
        LK = "st_lk"
        MK = "st_mk"
        HK = "st_hk"
        F_MP = "st_f_mp"
        B_MK = "st_b_mk"
        Guard = "st_guard"
        Turn = "st_turn"
    |}

    let Cr = {|
        Idle = "cr_idle"
        LP = "cr_lp"
        MP = "cr_mp"
        HP = "cr_hp"
        LK = "cr_lk"
        MK = "cr_mk"
        HK = "cr_hk"
        Guard = "cr_guard"
        Turn = "cr_turn"
    |}

    let Jump = {|
        Neutral = {|
            Start = "jump_n_start"
            Air = "jump_n_air"
        |}
        Forward = {|
            Start = "jump_f_start"
            Air = "jump_f_air"
        |}
        Backward = {|
            Start = "jump_b_start"
            Air = "jump_b_air"
        |}
        Landing = "jump_n_end"
        LK = "jump_lk"
        HK = "jump_hk"
        MK = "jump_mk"
        LP = "jump_lp"
        HP = "jump_hp"
        MP = "jump_mp"
    |}

    let HitStun = {|
        Stand = {|
            Legs = "hitstun_low"
            Body = "hitstun_mid"
            Head = "hitstun_high"
        |}
        Air = {| Flipout = "hitstun_air_flip" |}
        Crouch = "hitstun_cr"
        Crumple = "hitstun_crumple"
        Stagger = "hitstun_high_long"
        KnockDown = "hitstun_kd"
        KnockBack = "hitstun_knockback"
        KnockUp = "hitstun_knockup"
        WallBounce = "hitstun_wall_bounce"
        GroundBounce = "hitstun_ground_bounce"
    |}

    let LyingDown = {|
        FaceUp = "kd_idle_fu"
        FaceDown = "kd_idle_fd"
    |}

    let WakeUp = {|
        FaceUp = "wakeup_fu"
        FaceDown = "wakeup_fd"
        FaceUpQuick = "wakeup_fu_fast"
        RollForward = "roll_f"
    |}

    let Outro = {|
        Victory = "outro_win_1"
        Timeout = "outro_timeout"
    |}

    let RegularThrow = {|
        Escape = "throw_regular_escape"
        Forward = "throw_regular_start"
        ForwardExec = "throw_regular_connected_f"
        ForwardStun = "throw_regular_hurt_f"
    |}

    let Taunt = {| Hurry = "taunt_1" |}

    let Sp = {|
        Fireball = "sp_fireball"
        Uppercut = "sp_uppercut"
        DonkeyKick = "sp_donkey_kick"
        DiveKick = "sp_dive_kick"
        DiveKickLanding = "sp_dive_kick_landing"
        Tatsu = {|
            Start = "sp_tatsu_start"
            Loop = "sp_tatsu_loop"
            End = "sp_tatsu_end"
        |}
    |}
