﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using GitHub.Collections;
using GitHub.Models;
using ReactiveUI;

namespace GitHub.ViewModels.GitHubPane
{
    public abstract class IssueListViewModelBase : PanePageViewModelBase, IIssueListViewModelBase
    {
        IReadOnlyList<IViewModel> items;
        ICollectionView itemsView;
        string searchQuery;

        public IReadOnlyList<IViewModel> Items
        {
            get { return items; }
            private set { this.RaiseAndSetIfChanged(ref items, value); }
        }

        public ICollectionView ItemsView
        {
            get { return itemsView; }
            private set { this.RaiseAndSetIfChanged(ref itemsView, value); }
        }

        public ILocalRepositoryModel LocalRepository { get; private set; }

        public string SearchQuery
        {
            get { return searchQuery; }
            set { this.RaiseAndSetIfChanged(ref searchQuery, value); }
        }

        public async Task InitializeAsync(ILocalRepositoryModel repository, IConnection connection)
        {
            LocalRepository = repository;
            await Load();
        }

        protected abstract IVirtualizingListSource<IViewModel> CreateItemSource();

        Task Load()
        {
            var items = new VirtualizingList<IViewModel>(CreateItemSource(), null);
            Items = items;
            ItemsView = new VirtualizingListCollectionView<IViewModel>(items);

            return Task.CompletedTask;
        }
    }
}