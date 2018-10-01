﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contraindicator.Models.Nodes;
using Contraindicator.Models.Relationships;

namespace Contraindicator.Data
{
    public class GraphClientSeedData : IGraphClientSeedData
    {
        private readonly IGraphClientRepository _repository;

        public GraphClientSeedData(IGraphClientRepository repository)
        {
            _repository = repository;
        }
        public async Task EnsureSeedDataAsync()
        {
            if (!_repository.IsInitialized())
            {
                await _repository.InitializeAsync();

                //Seed Data
                var sub1 = await _repository.CreateSubstanceAsync(new Substance
                {
                    Name = "lysergic acid diethylamide",
                    Description = "Schedule I drugs, substances, or chemicals are defined as drugs with no currently accepted medical use and a high potential for abuse.",
                    Properties = null
                });

                var sub2 = await _repository.CreateSubstanceAsync(new Substance
                {
                    Name = "methylenedioxymethamphetamine",
                    Description = "Schedule I drugs, substances, or chemicals are defined as drugs with no currently accepted medical use and a high potential for abuse.",
                    Properties = null
                });

                var drug1 = await _repository.CreateProductAsync(new Product
                {
                    Name = "LSD",
                    Description = "Halocigenic",
                    Properties = null
                });

                var drug2 = await _repository.CreateProductAsync(new Product
                {
                    Name = "Ecstacy",
                    Description = "Party Drug",
                    Properties = null
                });

                await _repository.CreateContainAsync(drug1.Data.ProductId, sub1.Data.SubstanceId, new Contain());
                await _repository.CreateContainAsync(drug2.Data.ProductId, sub2.Data.SubstanceId, new Contain());
                await _repository.CreateContraindicateAsync(sub1.Data.SubstanceId, sub2.Data.SubstanceId, new Contraindicate());
            }
        }
    }
}
