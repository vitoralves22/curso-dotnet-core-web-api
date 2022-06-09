﻿using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyWallWebAPI.Domain.Models;
using MyWallWebAPI.Infrastructure.Data.Contexts;

namespace MyWallWebAPI.Infrastructure.Data.Repositories
{
    public class ChatRepository
    {
        private readonly MySQLContext _context;

        public ChatRepository(MySQLContext context)
        {
            _context = context;
        }


        public async Task<Chat> GetChatById(int ChatId)
        {
            Chat chat = await _context.Chat.Where(p => p.Id == ChatId).OrderBy(p => p.Data).Include(p => p.ChatUsers).Include(p => p.Messages).FirstOrDefaultAsync();

            return chat;
        }

        public async Task<List<ChatUser>> GetChatUsersByChatId(int ChatId)
        {
            List<ChatUser> chatUsers = await _context.ChatUser.Where(p => p.ChatId == ChatId).Include(p => p.ApplicationUser).ToListAsync();

            return chatUsers;
        }


        public async Task<Chat> CreateChat(Chat chat)
        {
            var ret = await _context.Chat.AddAsync(chat);

            await _context.SaveChangesAsync();

            ret.State = EntityState.Detached;

            return ret.Entity;
        }

        public async Task<List<ChatUser>> ListChatUsersByChatId(int Chatid)
        {
            List<ChatUser> list = await _context.ChatUser.Where(p => p.ChatId.Equals(Chatid)).ToListAsync();

           return list;
        }

        public async Task<List<ChatUser>> ListChatUsersByUserId(int Userid)
        {
            List<ChatUser> list = await _context.ChatUser.Where(p => p.ApplicationUserId.Equals(Userid)).ToListAsync();

            return list;
        }

    }
}