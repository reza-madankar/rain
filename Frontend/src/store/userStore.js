import { create } from 'zustand';
import { persist } from 'zustand/middleware';

const useUserStore = create(
  persist(
    (set, get) => ({
      userId: '',
      isFirstLoad: true,
      setUserId: (userId) => set({ userId, isFirstLoad: false }),
      clearUserId: () => set({ userId: '', isFirstLoad: true }),
      setFirstLoad: (isFirstLoad) => set({ isFirstLoad }),
    }),
    {
      name: 'user-storage', // unique name for localStorage key
    }
  )
);

export default useUserStore; 