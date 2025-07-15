import { CanDeactivateFn } from '@angular/router';
import { MemberProfile } from '../../features/members/member-profile/member-profile';

export const preventUnsaveChangesGuard: CanDeactivateFn<MemberProfile> = (component) => {
  if (component.editForm?.dirty) {
    const confirmLeave = window.confirm('You have unsaved changes. Do you really want to leave?');
    if (!confirmLeave) {  
      return false; // Prevent navigation
    }
  }
  return true;
};
