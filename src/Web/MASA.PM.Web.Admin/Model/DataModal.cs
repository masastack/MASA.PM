﻿// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.UI.Admin.Model
{
    public class DataModal<T> where T : class, new()
    {
        public bool Visible { get; set; }

        public T Data { get; set; }

        public bool HasValue { get; private set; }

        public bool Loading { get; private set; }

        public DataModal()
        {
            Data = new T();
        }

        public void Show()
        {
            Visible = true;
            HasValue = false;
        }

        /// <summary>
        /// 显示弹窗
        /// </summary>
        /// <param name="data"></param>
        /// <remarks>写入数据之前请确保此对象是拷贝过的</remarks>
        public void Show(T data)
        {
            Visible = true;
            HasValue = true;
            Data = data;
        }

        public virtual void Hide()
        {
            Visible = false;
            HasValue = false;
            Data = new T();
        }

        public bool ShowLoading() => Loading = true;

        public bool HideLoading() => Loading = false;
    }

    public class DataModal<T, D> : DataModal<T> where T : class, new()
    {
        public DataModal() : base()
        {
        }

        public DataModal(D depend) : base()
        {
            Depend = depend;
        }

        /// <summary>
        /// 依赖的数据
        /// </summary>
        public D? Depend { get; private set; }

        public void Show(D depend)
        {
            Visible = true;
            Depend = depend;
        }

        /// <summary>
        /// 显示弹窗
        /// </summary>
        /// <param name="data"></param>
        /// <param name="depned"></param>
        /// <remarks>写入数据之前请确保此对象是拷贝过的</remarks>
        public void Show(T data, D depned)
        {
            base.Show(data);
            Depend = depned;
        }

        public override void Hide()
        {
            base.Hide();
            Depend = default;
        }
    }
}
