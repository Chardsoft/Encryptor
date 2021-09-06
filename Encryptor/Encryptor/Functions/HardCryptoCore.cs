﻿using Encryptor.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encryptor.Functions
{
    public class HardCryptoCore : Functions.Function
    {
        protected string Path;
        protected byte[] Password1;
        protected byte[] Password2;
        protected int Difficulty;

        protected byte[] MixPasswords()
        {
            var mixed = new List<byte>();

            var biggerLen = Password1.Length > Password2.Length ? Password1.Length : Password2.Length;
            for (long i = 0; i < biggerLen; i++)
            {
                if (i < Password1.Length)
                    mixed.Add(Password1[i]);
                if (i < Password2.Length)
                    mixed.Add(Password2[i]);
            }

            return mixed.ToArray();
        }

        protected int GenerateNumericPassword()
        {
            return MixPasswords().OrderBy(b => b).Last();
        }

        protected long GenerateHashRounds(long currentRound) 
        {
            return currentRound * 1000;
        }

        protected byte[] GeneratePrivateKey(long currentRound)
        {
            var salt = MixPasswords();

            var saltedKey = new List<byte>();
            saltedKey.AddRange(Password1);
            saltedKey.AddRange(salt);

            var rounds = GenerateHashRounds(currentRound);
            var hashed = saltedKey.ToArray().SHA512(rounds);
            var finalHash = hashed.SHA256(rounds);

            return finalHash;
        }

        protected byte[] GenerateInitialVector(long currentRound)
        {
            var rounds = GenerateHashRounds(currentRound);
            var hash = Password2.SHA512(rounds);
            var finalHash = hash.SHA256(rounds).MD5(rounds);

            return finalHash;
        }

        protected long GenerateRounds() 
        {
            return GenerateNumericPassword() * Difficulty;
        }

        public override string GetHelp()
        {
            throw new NotImplementedException();
        }

        public override string GetName()
        {
            throw new NotImplementedException();
        }

        public override void Execute(string[] args)
        {
            throw new NotImplementedException();
        }
    }
}
