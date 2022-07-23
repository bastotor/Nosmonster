/*
 * This file is part of the OpenNos Emulator Project. See AUTHORS file for Copyright information
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 */

namespace OpenNos.Domain
{
    public enum AuthorityType : short
    {

        // Role qui marche bien
        Unconfirmed = -1,
        Banned = -2, // BAN
        BitchNiggerFaggot = -69, // MUTE
        User = 0, // RANG USER
        VIP = 1, // RANG DONATOR
        GameSage = 4, // RANG HELPER
        GameMaster = 50, // RANG GAME MASTER
        DEV = 60, // RANG DEVELOPER
        CommunityManager = 100, 
        SGM = 150, // RANG SUPER GAME MASTER
        Administrator = 200, // RANG ADMINISTRATOR
        CoFounder = 333,
        Founder = 666,



    }
}