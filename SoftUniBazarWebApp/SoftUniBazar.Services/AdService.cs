﻿namespace SoftUniBazar.Services
{
	using Microsoft.EntityFrameworkCore;
	using SoftUniBazar.Data;
	using SoftUniBazar.Models;
	using SoftUniBazar.Services.Interfaces;
	using SoftUniBazar.ViewModels;
	using System.Globalization;

	public class AdService : IAdService
	{
		private readonly BazarDbContext dbContext;

		public AdService(BazarDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public async Task AddingNewAdAsync(AdPostViewModel model, string userId)
		{
			Ad ad = new Ad
			{
				Name = model.Name,
				Description = model.Description,
				Price = model.Price,
				ImageUrl = model.ImageUrl,
				CreatedOn = DateTime.UtcNow,
				OwnerId = userId,
				CategoryId = model.CategoryId,
			};

			await dbContext.Ads.AddAsync(ad);
			await dbContext.SaveChangesAsync();
		}

		public async Task EditPostAsync(int adId, AdPostViewModel model, string userId)
		{
			Ad ad = await dbContext.Ads.FirstAsync(a => a.Id == adId);

			if (ad.OwnerId != userId)
			{
				throw new InvalidOperationException();
			}

			ad.Name = model.Name;
			ad.Description = model.Description;
			ad.ImageUrl = model.ImageUrl;
			ad.Price = model.Price;
			ad.CategoryId = model.CategoryId;

			await dbContext.SaveChangesAsync();
		}

		public async Task<ICollection<ListAllAdViewModel>> GetAllAdsAsync()
		{
			string dateFormat = "yyyy-MM-dd H:mm";

			ICollection<ListAllAdViewModel> allAds = await dbContext.Ads
				.Select(a => new ListAllAdViewModel
				{
					Id = a.Id,
					Name = a.Name,
					ImageUrl = a.ImageUrl,
					CreatedOn = a.CreatedOn.ToString(dateFormat, CultureInfo.InvariantCulture),
					Category = a.Category.Name,
					Description = a.Description,
					Price = a.Price.ToString("f2"),
					Owner = a.Owner.UserName
				})
				.ToArrayAsync();

			return allAds;
		}

		public async Task<ICollection<CategorySelectViewModel>> GetCategoriesAsync()
		{
			ICollection<CategorySelectViewModel> categories = await dbContext.Categories
				.Select(c => new CategorySelectViewModel
				{
					Id = c.Id,
					Name = c.Name,
				})
				.ToArrayAsync();

			return categories;
		}

		public async Task<AdPostViewModel> GetModelForEditAsync(int adId, string userId)
		{
			Ad ad = await dbContext.Ads.FirstAsync(a => a.Id == adId);

			if (ad.OwnerId != userId)
			{
				throw new InvalidOperationException();
			}

			AdPostViewModel model = new AdPostViewModel
			{
				Name = ad.Name,
				Description = ad.Description,
				ImageUrl = ad.ImageUrl,
				Price = ad.Price,
				CategoryId = ad.CategoryId,
			};

			model.Categories = await GetCategoriesAsync();

			return model;
		}
	}
}
