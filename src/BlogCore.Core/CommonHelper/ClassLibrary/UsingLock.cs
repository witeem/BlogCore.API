﻿// 创建人：魏天华 
// 测试添加代码文件头

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BlogCore.Core.CommonHelper.ClassLibrary
{
    /// <summary>
    /// 使用using代替lock操作的对象,可指定写入和读取锁定模式
    /// 参考:https://www.cnblogs.com/blqw/p/3475734.html
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class UsingLock<T> : IDisposable
    {
        #region 内部类

        /// <summary> 利用IDisposable的using语法糖方便的释放锁定操作
        /// <para>内部类</para>
        /// </summary>
        private struct Lock : IDisposable
        {
            /// <summary> 读写锁
            /// </summary>
            private ReaderWriterLockSlim _LockSlim = new ReaderWriterLockSlim();

            /// <summary> 是否为写入模式
            /// </summary>
            private bool _IsWrite;
            /// <summary> 利用IDisposable的using语法糖方便的释放锁定操作
            /// <para>构造函数</para>
            /// </summary>
            /// <param name="rwl">读写锁</param>
            /// <param name="isWrite">写入模式为true,读取模式为false</param>
            public Lock(ReaderWriterLockSlim rwl, bool isWrite)
            {
                _LockSlim = rwl;
                _IsWrite = isWrite;
            }

            /// <summary> 释放对象时退出指定锁定模式
            /// </summary>
            public void Dispose()
            {
                if (_IsWrite)
                {
                    if (_LockSlim.IsWriteLockHeld)
                    {
                        _LockSlim.ExitWriteLock();
                    }
                }
                else
                {
                    if (_LockSlim.IsReadLockHeld)
                    {
                        _LockSlim.ExitReadLock();
                    }
                }
            }
        }
        #endregion

        /// <summary> 读写锁
        /// </summary>
        private ReaderWriterLockSlim _LockSlim = new ReaderWriterLockSlim();

        /// <summary> 使用using代替lock操作的对象,可指定写入和读取锁定模式
        /// <para>构造函数</para>
        /// </summary>
        public UsingLock()
        {
            Enabled = true;
        }

        /// <summary> 是否启用,当该值为false时,Read()和Write()方法将返回 Disposable.Empty
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary> 进入读取锁定模式,该模式下允许多个读操作同时进行
        /// <para>退出读锁请将返回对象释放,建议使用using语块</para>
        /// <para>Enabled为false时,返回Disposable.Empty;</para>
        /// <para>在读取或写入锁定模式下重复执行,返回Disposable.Empty;</para>
        /// </summary>
        public IDisposable Read()
        {
            if (Enabled == false || _LockSlim.IsReadLockHeld || _LockSlim.IsWriteLockHeld)
            {
                return Disposable.Empty;
            }

            _LockSlim.EnterReadLock();
            return new Lock(_LockSlim, false);
        }

        /// <summary> 进入写入锁定模式,该模式下只允许同时执行一个读操作
        /// <para>退出读锁请将返回对象释放,建议使用using语块</para>
        /// <para>Enabled为false时,返回Disposable.Empty;</para>
        /// <para>在写入锁定模式下重复执行,返回Disposable.Empty;</para>
        /// </summary>
        /// <exception cref="NotImplementedException">读取模式下不能进入写入锁定状态</exception>
        public IDisposable Write()
        {
            if (Enabled == false || _LockSlim.IsWriteLockHeld)
            {
                return Disposable.Empty;
            }

            if (_LockSlim.IsReadLockHeld)
            {
                throw new NotImplementedException("读取模式下不能进入写入锁定状态");
            }

            _LockSlim.EnterWriteLock();
            return new Lock(_LockSlim, true);
        }

        public void Dispose()
        {
            _LockSlim.Dispose();
        }

        /// <summary> 空的可释放对象,免去了调用时需要判断是否为null的问题
        /// <para>内部类</para>
        /// </summary>
        private class Disposable : IDisposable
        {
            /// <summary> 空的可释放对象
            /// </summary>
            public static readonly Disposable Empty = new Disposable();
            /// <summary> 空的释放方法
            /// </summary>
            public void Dispose() { }
        }
    }
}
