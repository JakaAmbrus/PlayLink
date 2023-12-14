import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class CacheManagerService {
  private cache = new Map<string, any>();

  setCache(key: string, data: any): void {
    this.cache.set(key, data);
  }

  getCache<T>(key: string): T | null {
    return this.cache.has(key) ? (this.cache.get(key) as T) : null;
  }

  clearCache(key: string): void {
    this.cache.delete(key);
  }

  clearAllCache(): void {
    this.cache.clear();
  }
}
