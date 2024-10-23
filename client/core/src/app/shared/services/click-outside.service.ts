import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ClickOutsideService {
  private bindings = new Map<any, () => void>();

  bind(target: any, callback: () => void): void {
    this.unbind(target);
    const boundCallback = callback.bind(target);
    this.bindings.set(target, boundCallback);
    setTimeout(() => {
      document.addEventListener('click', boundCallback);
    });
  }

  unbind(target: any): void {
    const callback = this.bindings.get(target);
    if (callback) {
      document.removeEventListener('click', callback);
      this.bindings.delete(target);
    }
  }
}
